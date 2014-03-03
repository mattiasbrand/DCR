using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.Serialization;
using System.Xml;

namespace Common
{
    public class ReflectionDataContractResolver : DataContractResolver
    {
        public override bool TryResolveType(Type type, Type declaredType, DataContractResolver knownTypeResolver, out XmlDictionaryString typeName, out XmlDictionaryString typeNamespace)
        {
            if (!knownTypeResolver.TryResolveType(type, declaredType, null, out typeName, out typeNamespace))
            {
                var contract = type.GetCustomAttribute<DataContractAttribute>();
                if (contract != null)
                {
                    var dict = new XmlDictionary();
                    typeName = dict.Add(contract.Name);
                    typeNamespace = dict.Add(contract.Namespace);
                    return true;
                }

                return false;
            }

            return true; //knowntype went well
        }

        public override Type ResolveName(string typeName, string typeNamespace, Type declaredType, DataContractResolver knownTypeResolver)
        {
            Debugger.Launch();
            var resolvedType = knownTypeResolver.ResolveName(typeName, typeNamespace, declaredType, null) ?? GetTypeFromContractName(typeName, typeNamespace);
            return resolvedType;
        }

        public static Type GetTypeFromContractName(string contractName, string contractNamespace)
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                try
                {
                    foreach (var type in assembly.GetTypes())
                    {
                        var contract = type.GetCustomAttribute<DataContractAttribute>(true);
                        if (contract != null && contract.Name == contractName && contract.Namespace == contractNamespace)
                            return type;
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                }
            }

            return null;
        }


    }

}