using System.ServiceModel;
using Common;

namespace Server
{
    [ServiceContract(Name = "IDynamicContract", Namespace = "http://my.dynamic.contract", CallbackContract = typeof(IDynamicCallBack))]
    public interface IDynamicContract
    {
        [OperationContract]
        MyHolder EchoHolder(MyHolder holder);
        [OperationContract]
        MyHolder GetHolder(string typeName, string[] propertyNames, object[] propertyValues);
        [OperationContract]
        string PutHolder(MyHolder holder);
    }

    public interface IDynamicCallBack
    {
        [OperationContract(IsOneWay = true)]
        void OnNotificationSend(string message);
    }
}
