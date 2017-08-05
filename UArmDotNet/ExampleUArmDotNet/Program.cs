using System;
using System.Linq;
using System.Threading.Tasks;
using Baku.UArmDotNet;

namespace ExampleUArmDotNet
{
    class Program
    {
        static void Main(string[] args)
        {
            MainRoutine().Wait();
        }

        static async Task MainRoutine()
        {
            //search target serial port names
            string target = UArmSearch.SearchPortNames().FirstOrDefault();
            if (target == null)
            {
                Console.WriteLine("There seems to be no uArm, Program ends...");
                return;
            }
            else
            {
                Console.WriteLine("Try to connect to: " + target);
            }
            //NOTE: off course you can initialize by simpler way if COM port is already known:
            //var uArm = new UArm("COM1");
            var uArm = new UArm(target);

            //use "RawData" event handler to check (almost) raw communication
            uArm.Connector.ReceivedRawData += (_, e) => Console.WriteLine("Recv: " + e.RawData);
            uArm.Connector.SendRawData += (_, e) => Console.WriteLine("Send: " + e.RawData);

            uArm.Connector.Connect();

            //NOTE: By using "Connector.Post" you can send a dummy message.
            //  In my environment, 1st message (somehow) begin with "????" prefix (so any command is treated as Bad Format).
            //  To avoid bad format message error, send dummy command at first.
            uArm.Connector.Post("Hello");

            try
            {
                Console.WriteLine("SELECT TEST CONTENT(1/4): Test query commands? (y/N)");
                if (string.Compare("Y", Console.ReadLine(), true) == 0)
                {
                    await TestQueryCommands(uArm);
                }

                Console.WriteLine("SELECT TEST CONTENT(2/4): Test setting commands? (y/N)");
                Console.WriteLine("  *NOTE: include output command and gripper/suction cup will work, be careful for the safety!");
                if (string.Compare("Y", Console.ReadLine(), true) == 0)
                {
                    await TestNoMotionCommands(uArm);
                }

                Console.WriteLine("SELECT TEST CONTENT(3/4): Test motion commands? (y/N)");
                Console.WriteLine("  *NOTE: include motion command, be careful for the safety!");
                if (string.Compare("Y", Console.ReadLine(), true) == 0)
                {
                    await TestMotionCommands(uArm);
                }

                Console.WriteLine("SELECT TEST CONTENT(4/4): Test events? (y/N)");
                if (string.Compare("Y", Console.ReadLine(), true) == 0)
                {
                    await TestEvents(uArm);
                }
            }
            catch (Exception)
            {
                // Some exception ? please check on your environment 
            }

            uArm.Connector.Disconnect();
        }

        //Test & Example to call getter commands
        //NOTE: this test only reads current status of the robot, so does not include output commands
        static async Task TestQueryCommands(IUArm uArm)
        {
            // System information
            Console.WriteLine("Dev Name: " + await uArm.GetDeviceNameAsync());
            Console.WriteLine("H/W ver : " + await uArm.GetHardwareVersionAsync());
            Console.WriteLine("S/W ver : " + await uArm.GetSoftwareVersionAsync());
            Console.WriteLine("API ver : " + await uArm.GetAPIVersionAsync());
            Console.WriteLine("UID     : " + await uArm.GetUIDAsync());

            // Position-related information

            // API to get single servo angle
            Console.WriteLine($"Angle (single): B = {await uArm.GetServoAngleAsync(Servos.Bottom)}");
            Console.WriteLine($"Angle (single): L = {await uArm.GetServoAngleAsync(Servos.Left)}");
            Console.WriteLine($"Angle (single): R = {await uArm.GetServoAngleAsync(Servos.Right)}");

            // API to get all servo angle (except hand)
            ServoAngles angles = await uArm.GetAllServoAnglesAsync();
            Console.WriteLine($"Angles: B={angles.Bottom}, L={angles.Left}, R={angles.Right}");

            Position pos = await uArm.GetPositionAsync();
            Console.WriteLine($"Pos   : X={pos.X}, Y={pos.Y}, Z={pos.Z}");

            Polar polar = await uArm.GetPolarAsync();
            Console.WriteLine($"Polar : S={polar.Stretch}, R={polar.Rotation}, H={polar.Height}");

            // NOTE: this data is NOT the angle (seems to be pulse value)
            ServoAngles defaultPulse = await uArm.GetDefaultValueOfAS5600Async();
            Console.WriteLine($"Default Pulse: B={defaultPulse.Bottom}, L={defaultPulse.Left}, R={defaultPulse.Right}");

            // I/O-related information
            Console.WriteLine($"Pump state         : {await uArm.GetPumpStatusAsync()}");
            Console.WriteLine($"Gripper state      : {await uArm.GetGripperStatusAsync()}");
            Console.WriteLine($"Digital In 0 High? : {await uArm.GetDigitalPinStateAsync(0)}");
            Console.WriteLine($"Analog In 0 ADC    : {await uArm.GetAnalogPinStateAsync(0)}");
            Console.WriteLine($"Limited Switch Triggered? : {await uArm.CheckLimitedSwitchTriggeredAsync()}");

            // "Check" commands, classified in checking 
            Console.WriteLine($"Robot is Moving? : {await uArm.CheckIsMovingAsync()}");

            Console.Write("Motor Attached? ");
            Console.Write($"M0: {await uArm.CheckMotorAttachedAsync(Servos.Bottom)}, ");
            Console.Write($"M1: {await uArm.CheckMotorAttachedAsync(Servos.Left)}, ");
            Console.Write($"M2: {await uArm.CheckMotorAttachedAsync(Servos.Right)}, ");
            Console.WriteLine($"M3: {await uArm.CheckMotorAttachedAsync(Servos.Hand)}");
        }

        //Test & Example to call setter commands, except robot's motion
        // CAUTION: some operation includes output
        // - Beep (outputs some sound)
        // - Motor ON/OFF
        // - Suction cup ON/OFF
        // - Gripper ON/OFF
        // - Digital Out 0 ON/OFF
        static async Task TestNoMotionCommands(IUArm uArm)
        {
            await uArm.BeepAsync(440, 50);

            Console.WriteLine("Detach All Motors");
            await uArm.DetachAllMotorAsync();
            await Task.Delay(500);
            Console.WriteLine("Attach All Motors");
            await uArm.AttachAllMotorAsync();
            await Task.Delay(500);

            Console.WriteLine("Detach All Motors");
            await uArm.DetachMotorAsync(Servos.Bottom);
            await uArm.DetachMotorAsync(Servos.Left);
            await uArm.DetachMotorAsync(Servos.Right);
            await Task.Delay(500);

            Console.WriteLine("Attach All Motors");
            await uArm.AttachMotorAsync(Servos.Bottom);
            await uArm.AttachMotorAsync(Servos.Left);
            await uArm.AttachMotorAsync(Servos.Right);
            await Task.Delay(500);

            // Pos -> Angles by IK -> Pos by FK (Math)
            var pos = new Position(100, 100, 100);
            var angles = await uArm.GetIKAsync(pos);
            Console.WriteLine($"IK, src: X={pos.X}, Y={pos.Y}, Z={pos.Z}");
            Console.WriteLine($"IK, res: L={angles.Bottom}, B={angles.Left}, R={angles.Right}");

            pos = await uArm.GetFKAsync(angles);
            Console.WriteLine($"FK, src: L={angles.Bottom}, B={angles.Left}, R={angles.Right}");
            Console.WriteLine($"FK, res: X={pos.X}, Y={pos.Y}, Z={pos.Z}");

            // Can Reach or not
            pos = new Position(100, 100, 100);
            Console.WriteLine($"Can reach to (x,y,z)=({pos.X}, {pos.Y}, {pos.Z})? : {await uArm.CanReachAsync(pos)}");
            pos = new Position(300, 300, 100);
            Console.WriteLine($"Can reach to (x,y,z)=({pos.X}, {pos.Y}, {pos.Z})? : {await uArm.CanReachAsync(pos)}");

            var polar = new Polar(100, 45, 100);
            Console.WriteLine($"Can reach to (s,r,h)=({polar.Stretch}, {polar.Rotation}, {polar.Height})? : {await uArm.CanReachAsync(polar)}");
            polar = new Polar(400, 200, 100);
            Console.WriteLine($"Can reach to (s,r,h)=({polar.Stretch}, {polar.Rotation}, {polar.Height})? : {await uArm.CanReachAsync(polar)}");

            Console.WriteLine("Activate and deactivate the pump");
            await Task.Delay(1000);            
            await uArm.SetPumpStateAsync(true);
            await Task.Delay(1000);
            await uArm.SetPumpStateAsync(false);

            Console.WriteLine("Activate and deactivate the gripper");
            await Task.Delay(1000);
            await uArm.SetGripperStateAsync(true);
            await Task.Delay(1000);
            await uArm.SetGripperStateAsync(false);

            // NO bluetooth operation, because when using this, serial connection will be shut down!
            //Console.WriteLine("Activate and deactivate the bluetooth");
            //await Task.Delay(1000);
            //await uArm.SetBluetoothStateAsync(true);
            //await Task.Delay(1000);
            //await uArm.SetBluetoothStateAsync(false);

            Console.WriteLine("Change digital Out 0 to high, and low");
            await Task.Delay(1000);
            await uArm.SetDigitalPinOutputAsync(0, true);
            await Task.Delay(1000);
            await uArm.SetDigitalPinOutputAsync(0, false);

            Console.WriteLine("Change ArmMode to Normal, Laser, Printing, Universal Holder, and then Normal again");
            await Task.Delay(500);
            await uArm.SetArmModeAsync(ArmModes.Normal);
            await Task.Delay(500);
            await uArm.SetArmModeAsync(ArmModes.Laser);
            await Task.Delay(500);
            await uArm.SetArmModeAsync(ArmModes.Printing);
            await Task.Delay(500);
            await uArm.SetArmModeAsync(ArmModes.UniversalHolder);
            await Task.Delay(500);
            await uArm.SetArmModeAsync(ArmModes.Normal);

            Console.WriteLine("Set Current Position to the reference position");
            try
            {
                await uArm.UpdateReferencePoint();
            }
            catch(UArmException ex)
            {
                Console.WriteLine("Expected error, message: " + ex.Message);
            }

            Console.WriteLine("Set Height zero Point");
            await uArm.UpdateHeightZeroPoint();

            Console.WriteLine("Set End Effector Height 100mm, and 0mm");
            await uArm.SetEndEffectorHeight(100);
            await uArm.SetEndEffectorHeight(0);

        }

        //Test & Example to move robots
        static async Task TestMotionCommands(IUArm uArm)
        {            
            Console.WriteLine("NOTE: Before testing the motion of the robot, ");
            Console.WriteLine("  - Move the robot arm to some position, such that the robot can move to any direction, by 50mm");
            Console.WriteLine("  - Recommend to detach the heavy accessories like laser module, (for the case of test failure)");
            Console.WriteLine("  - Check that the hand does not hit the robot itself with any rotation (no extremely large hand equipped)");
            
            Console.WriteLine("Press ENTER to off the motor (be careful: the arm go down by the gravity!)");
            Console.ReadLine();
            await uArm.DetachAllMotorAsync();
            await Task.Delay(500);

            Console.WriteLine("After moving the robot, press ENTER to on the motor (*robot does not move immediately, there is one more confirmation step)");
            Console.ReadLine();
            await uArm.AttachAllMotorAsync();
            await Task.Delay(500);

            Console.WriteLine("Take a distance from robot, then press ENTER to start the motion test");
            Console.ReadLine();

            Console.WriteLine("Start motion test..");
            //Get base state
            var currentPos = await uArm.GetPositionAsync();
            var currentAngles = await uArm.GetAllServoAnglesAsync();

            Console.WriteLine($"Current Pos  : {currentPos}");
            Console.WriteLine($"Current Angle: {currentAngles}");

            #region Relative XYZ
            Console.WriteLine("Move by relative XYZ motion");

            Console.WriteLine("+X");
            await uArm.MoveRelativeAsync(new Position(50, 0, 0), 500);
            await Task.Delay(100);
            Console.WriteLine("-X");
            await uArm.MoveRelativeAsync(new Position(-50, 0, 0), 500);
            await Task.Delay(100);

            Console.WriteLine("+Y");
            await uArm.MoveRelativeAsync(new Position(0, 50, 0), 500);
            await Task.Delay(100);
            Console.WriteLine("-Y");
            await uArm.MoveRelativeAsync(new Position(0, -50, 0), 500);
            await Task.Delay(100);

            Console.WriteLine("-Z");
            await uArm.MoveRelativeAsync(new Position(0, 0, 50), 500);
            await Task.Delay(100);
            Console.WriteLine("-Z");
            await uArm.MoveRelativeAsync(new Position(0, 0, -50), 500);
            await Task.Delay(100);

            Console.WriteLine($"Current Pos  : {await uArm.GetPositionAsync()}");
            Console.WriteLine($"Current Angle: {await uArm.GetAllServoAnglesAsync()}");
            #endregion

            #region Relative Polar(SRH)
            Console.WriteLine("Move by relative Polar(SRH) motion");

            Console.WriteLine("+S");
            await uArm.MoveRelativeAsync(new Polar(50, 0, 0), 500);
            await Task.Delay(100);
            Console.WriteLine("-S");
            await uArm.MoveRelativeAsync(new Polar(-50, 0, 0), 500);
            await Task.Delay(100);

            Console.WriteLine("+R");
            await uArm.MoveRelativeAsync(new Polar(0, 10, 0), 500);
            await Task.Delay(100);
            Console.WriteLine("-R");
            await uArm.MoveRelativeAsync(new Polar(0, -10, 0), 500);
            await Task.Delay(100);

            Console.WriteLine("-H");
            await uArm.MoveRelativeAsync(new Polar(0, 0, 50), 500);
            await Task.Delay(100);
            Console.WriteLine("-H");
            await uArm.MoveRelativeAsync(new Polar(0, 0, -50), 500);
            await Task.Delay(100);

            Console.WriteLine($"Current Pos  : {await uArm.GetPositionAsync()}");
            Console.WriteLine($"Current Angle: {await uArm.GetAllServoAnglesAsync()}");
            #endregion

            #region Absolute Angle(Bottom, Left, Right, Hand)
            Console.WriteLine("Move by absolute angle(BLR, and Hand) input");

            Console.WriteLine("Bottom +/-");
            await uArm.MoveServoAngleAsync(Servos.Bottom, currentAngles.Bottom + 10.0f);
            await Task.Delay(500);
            await uArm.MoveServoAngleAsync(Servos.Bottom, currentAngles.Bottom);
            await Task.Delay(500);

            Console.WriteLine("Left +/-");
            await uArm.MoveServoAngleAsync(Servos.Left, currentAngles.Left + 10.0f);
            await Task.Delay(500);
            await uArm.MoveServoAngleAsync(Servos.Left, currentAngles.Left);
            await Task.Delay(500);

            Console.WriteLine("Right +/-");
            await uArm.MoveServoAngleAsync(Servos.Right, currentAngles.Right + 10.0f);
            await Task.Delay(500);
            await uArm.MoveServoAngleAsync(Servos.Right, currentAngles.Right);
            await Task.Delay(500);

            Console.WriteLine("Hand 0deg, 45deg, 135deg, 180deg, and 90deg");
            await uArm.MoveServoAngleAsync(Servos.Hand, 0);
            await Task.Delay(500);
            await uArm.MoveServoAngleAsync(Servos.Hand, 45);
            await Task.Delay(500);
            await uArm.MoveServoAngleAsync(Servos.Hand, 135);
            await Task.Delay(500);
            await uArm.MoveServoAngleAsync(Servos.Hand, 180);
            await Task.Delay(500);
            await uArm.MoveServoAngleAsync(Servos.Hand, 90);
            await Task.Delay(500);

            #endregion

            #region Absolute XYZ / Polar

            Console.WriteLine("Move by absolute XYZ motion");

            await uArm.MoveAsync(new Position(currentPos.X + 50, currentPos.Y + 50, currentPos.Z + 50), 500);
            await Task.Delay(100);
            await uArm.MoveAsync(currentPos, 500);
            await Task.Delay(100);

            Console.WriteLine("Move by absolute Polar motion");

            var currentPolar = await uArm.GetPolarAsync();
            await uArm.MoveAsync(
                new Polar(currentPolar.Stretch + 50, currentPolar.Rotation + 10, currentPolar.Height + 50), 500
                );
            await Task.Delay(100);
            await uArm.MoveAsync(currentPolar, 500);
            await Task.Delay(100);

            #endregion

        }

        static async Task TestEvents(IUArm uArm)
        {
            //Console.WriteLine("Disable default function of the base buttons");
            //await uArm.SetEnableDefaultFunctionOfBaseButtonsAsync(false);

            Console.WriteLine("Enable event handler loggers");
            uArm.LimitedSwitchStateChanged += OnLimitedSwitchStateChanged;
            uArm.PowerConnectionChanged += OnPowerConnectionChanged;
            uArm.ReceivedButtonAction += OnReceivedButtonAction;
            uArm.ReceivedPositionFeedback += OnReceivedPositionFeedback;

            while (true)
            {
                Console.WriteLine("Input 'F' to start 1.0sec freq feedback, 'D' to disable, or end by other pattern");
                string line = Console.ReadLine();
                if (string.Compare("F", line, true) == 0)
                {
                    Console.WriteLine("start feedback");
                    await uArm.StartFeedbackCycleAsync(1.0f);
                    Console.WriteLine("wait 5sec, and see the feedback example");
                    await Task.Delay(5000);
                }
                else if (string.Compare("D", line, true) == 0)
                {
                    Console.WriteLine("stop feedback");
                    await uArm.StartFeedbackCycleAsync(0.0f);
                }
                else
                {
                    break;
                }
            }

            Console.WriteLine("Disable event handler loggers");
            uArm.LimitedSwitchStateChanged -= OnLimitedSwitchStateChanged;
            uArm.PowerConnectionChanged -= OnPowerConnectionChanged;
            uArm.ReceivedButtonAction -= OnReceivedButtonAction;
            uArm.ReceivedPositionFeedback -= OnReceivedPositionFeedback;

            //Console.WriteLine("Enable default function of the base buttons");
            //await uArm.SetEnableDefaultFunctionOfBaseButtonsAsync(true);
        }

        private static void OnReceivedPositionFeedback(object sender, PositionFeedbackEventArgs e)
        {
            Console.WriteLine($"  EVENT: Position Feedback, position: {e.Position}, hand angle:{e.HandAngle}");
        }
        private static void OnReceivedButtonAction(object sender, ButtonActionEventArgs e)
        {
            Console.WriteLine($"  EVENT: Button Action, button={e.ButtonType}, action={e.ButtonAction}");
        }
        private static void OnPowerConnectionChanged(object sender, PowerConnectionChangedEventArgs e)
        {
            Console.WriteLine($"  EVENT: Power Connection, connected?={e.Connected}");
        }
        private static void OnLimitedSwitchStateChanged(object sender, LimitedSwitchEventArgs e)
        {
            Console.WriteLine($"  EVENT: Limited Switch State Changed, Number={e.SwitchNumber}, Triggered?={e.IsTriggerd}");
        }

        static Task TestEEPROMCommands(IUArm uArm)
        {
            //to be implemented.
            return Task.CompletedTask;
        }
    }
}
