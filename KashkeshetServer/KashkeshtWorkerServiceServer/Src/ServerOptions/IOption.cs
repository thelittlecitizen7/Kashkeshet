using KashkeshetCommon.Models.ChatData;

namespace KashkeshtWorkerServiceServer.Src.ServerOptions
{
    public interface IOption
    {
        void Operation(MainRequest chatData);
    }
}
