namespace Baku.UArmDotNet
{
    public class UArmEventMessage
    {
        public UArmEventMessage(int id, string[] args)
        {
            Id = id;
            Args = args;
        }

        /// <summary>Get the Id bound to the command sent from this client.</summary>
        public int Id { get; }

        /// <summary>Get the raw response string data</summary>
        public string[] Args { get; }


    }
}
