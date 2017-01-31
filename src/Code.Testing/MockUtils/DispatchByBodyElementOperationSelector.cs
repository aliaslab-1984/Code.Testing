using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Code.Testing.MockUtils
{
    public class DispatchByBodyElementOperationSelector : IDispatchOperationSelector
    {
        List<string> dispatchDictionary;
        string defaultOperationName;

        public DispatchByBodyElementOperationSelector(List<string> dispatchDictionary, string defaultOperationName)
        {
            this.dispatchDictionary = dispatchDictionary;
            this.defaultOperationName = defaultOperationName;
        }

        #region IDispatchOperationSelector Members

        private Message CreateMessageCopy(Message message, XmlDictionaryReader body)
        {
            Message copy = Message.CreateMessage(message.Version, message.Headers.Action, body);
            copy.Headers.CopyHeaderFrom(message, 0);
            copy.Properties.CopyProperties(message.Properties);
            return copy;
        }

        public string SelectOperation(ref System.ServiceModel.Channels.Message message)
        {
            XmlDictionaryReader bodyReader = message.GetReaderAtBodyContents();
            XmlQualifiedName lookupQName = new XmlQualifiedName(bodyReader.LocalName, bodyReader.NamespaceURI);
            message = CreateMessageCopy(message, bodyReader);

            var operationName = dispatchDictionary.FirstOrDefault(e => lookupQName.Name.ToLower().Contains(e.ToLower()));

            if (operationName != null)
            {
                return operationName;
            }

            return defaultOperationName;
        }

        #endregion
    }
}
