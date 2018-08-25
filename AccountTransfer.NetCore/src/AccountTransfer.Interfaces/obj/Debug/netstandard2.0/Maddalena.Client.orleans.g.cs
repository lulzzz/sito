// <auto-generated />
#if !EXCLUDE_CODEGEN
#pragma warning disable 162
#pragma warning disable 219
#pragma warning disable 414
#pragma warning disable 618
#pragma warning disable 649
#pragma warning disable 693
#pragma warning disable 1591
#pragma warning disable 1998
[assembly: global::Orleans.Metadata.FeaturePopulatorAttribute(typeof (OrleansGeneratedCode.OrleansCodeGenf819cada42FeaturePopulator))]
[assembly: global::System.CodeDom.Compiler.GeneratedCodeAttribute(@"Orleans-CodeGenerator", @"2.0.0.0")]
[assembly: global::Orleans.CodeGeneration.OrleansCodeGenerationTargetAttribute(@"Maddalena.Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
namespace Maddalena.Client.Interfaces
{
    using global::Orleans;
    using global::System.Reflection;

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute(@"Orleans-CodeGenerator", @"2.0.0.0"), global::System.SerializableAttribute, global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute, global::Orleans.CodeGeneration.GrainReferenceAttribute(typeof (global::Maddalena.Client.Interfaces.IFeedGrain))]
    internal class OrleansCodeGenFeedGrainReference : global::Orleans.Runtime.GrainReference, global::Maddalena.Client.Interfaces.IFeedGrain
    {
        protected OrleansCodeGenFeedGrainReference(global::Orleans.Runtime.GrainReference other): base (other)
        {
        }

        OrleansCodeGenFeedGrainReference(global::Orleans.Runtime.GrainReference other, global::Orleans.CodeGeneration.InvokeMethodOptions invokeMethodOptions): base (other, invokeMethodOptions)
        {
        }

        protected OrleansCodeGenFeedGrainReference(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context): base (info, context)
        {
        }

        public override global::System.Int32 InterfaceId
        {
            get
            {
                return -1642422289;
            }
        }

        public override global::System.UInt16 InterfaceVersion
        {
            get
            {
                return 1;
            }
        }

        public override global::System.String InterfaceName
        {
            get
            {
                return @"global::Maddalena.Client.Interfaces.IFeedGrain";
            }
        }

        public override global::System.Boolean IsCompatible(global::System.Int32 interfaceId)
        {
            return interfaceId == -1642422289;
        }

        public override global::System.String GetMethodName(global::System.Int32 interfaceId, global::System.Int32 methodId)
        {
            switch (interfaceId)
            {
                case -1642422289:
                    switch (methodId)
                    {
                        case 2003862074:
                            return @"SetupReminderAsync";
                        default:
                            throw new global::System.NotImplementedException(@"interfaceId=" + -1642422289 + @",methodId=" + methodId);
                    }

                default:
                    throw new global::System.NotImplementedException(@"interfaceId=" + interfaceId);
            }
        }

        public global::System.Threading.Tasks.Task SetupReminderAsync()
        {
            return base.InvokeMethodAsync<global::System.Object>(2003862074, null);
        }
    }

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute(@"Orleans-CodeGenerator", @"2.0.0.0"), global::Orleans.CodeGeneration.MethodInvokerAttribute(typeof (global::Maddalena.Client.Interfaces.IFeedGrain), -1642422289), global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute]
    internal class OrleansCodeGenFeedGrainMethodInvoker : global::Orleans.CodeGeneration.IGrainMethodInvoker
    {
        public async global::System.Threading.Tasks.Task<global::System.Object> Invoke(global::Orleans.Runtime.IAddressable grain, global::Orleans.CodeGeneration.InvokeMethodRequest request)
        {
            global::System.Int32 interfaceId = request.InterfaceId;
            global::System.Int32 methodId = request.MethodId;
            global::System.Object[] arguments = request.Arguments;
            if (grain == null)
                throw new global::System.ArgumentNullException(@"grain");
            switch (interfaceId)
            {
                case -1642422289:
                    switch (methodId)
                    {
                        case 2003862074:
                            await ((global::Maddalena.Client.Interfaces.IFeedGrain)grain).SetupReminderAsync();
                            return null;
                        default:
                            throw new global::System.NotImplementedException(@"interfaceId=" + -1642422289 + @",methodId=" + methodId);
                    }

                default:
                    throw new global::System.NotImplementedException(@"interfaceId=" + interfaceId);
            }
        }

        public global::System.Int32 InterfaceId
        {
            get
            {
                return -1642422289;
            }
        }

        public global::System.UInt16 InterfaceVersion
        {
            get
            {
                return 1;
            }
        }
    }

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute(@"Orleans-CodeGenerator", @"2.0.0.0"), global::System.SerializableAttribute, global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute, global::Orleans.CodeGeneration.GrainReferenceAttribute(typeof (global::Maddalena.Client.Interfaces.INewsGrain))]
    internal class OrleansCodeGenNewsGrainReference : global::Orleans.Runtime.GrainReference, global::Maddalena.Client.Interfaces.INewsGrain
    {
        protected OrleansCodeGenNewsGrainReference(global::Orleans.Runtime.GrainReference other): base (other)
        {
        }

        OrleansCodeGenNewsGrainReference(global::Orleans.Runtime.GrainReference other, global::Orleans.CodeGeneration.InvokeMethodOptions invokeMethodOptions): base (other, invokeMethodOptions)
        {
        }

        protected OrleansCodeGenNewsGrainReference(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context): base (info, context)
        {
        }

        public override global::System.Int32 InterfaceId
        {
            get
            {
                return 271477613;
            }
        }

        public override global::System.UInt16 InterfaceVersion
        {
            get
            {
                return 1;
            }
        }

        public override global::System.String InterfaceName
        {
            get
            {
                return @"global::Maddalena.Client.Interfaces.INewsGrain";
            }
        }

        public override global::System.Boolean IsCompatible(global::System.Int32 interfaceId)
        {
            return interfaceId == 271477613;
        }

        public override global::System.String GetMethodName(global::System.Int32 interfaceId, global::System.Int32 methodId)
        {
            switch (interfaceId)
            {
                case 271477613:
                    switch (methodId)
                    {
                        case 458662693:
                            return @"Create";
                        case 2070916615:
                            return @"GetNews";
                        case 1078770888:
                            return @"NewsInLabel";
                        case 1336951808:
                            return @"Update";
                        case 1088453568:
                            return @"Delete";
                        default:
                            throw new global::System.NotImplementedException(@"interfaceId=" + 271477613 + @",methodId=" + methodId);
                    }

                default:
                    throw new global::System.NotImplementedException(@"interfaceId=" + interfaceId);
            }
        }

        public global::System.Threading.Tasks.Task Create(global::Maddalena.Client.News news)
        {
            return base.InvokeMethodAsync<global::System.Object>(458662693, new global::System.Object[]{news});
        }

        public global::System.Threading.Tasks.Task<global::Maddalena.Client.News[]> GetNews()
        {
            return base.InvokeMethodAsync<global::Maddalena.Client.News[]>(2070916615, null);
        }

        public global::System.Threading.Tasks.Task<global::Maddalena.Client.News[]> NewsInLabel(global::System.String label, global::Maddalena.Client.LabelValue @value)
        {
            return base.InvokeMethodAsync<global::Maddalena.Client.News[]>(1078770888, new global::System.Object[]{label, @value});
        }

        public global::System.Threading.Tasks.Task Update(global::Maddalena.Client.News news)
        {
            return base.InvokeMethodAsync<global::System.Object>(1336951808, new global::System.Object[]{news});
        }

        public global::System.Threading.Tasks.Task Delete(global::Maddalena.Client.News news)
        {
            return base.InvokeMethodAsync<global::System.Object>(1088453568, new global::System.Object[]{news});
        }
    }

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute(@"Orleans-CodeGenerator", @"2.0.0.0"), global::Orleans.CodeGeneration.MethodInvokerAttribute(typeof (global::Maddalena.Client.Interfaces.INewsGrain), 271477613), global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute]
    internal class OrleansCodeGenNewsGrainMethodInvoker : global::Orleans.CodeGeneration.IGrainMethodInvoker
    {
        public async global::System.Threading.Tasks.Task<global::System.Object> Invoke(global::Orleans.Runtime.IAddressable grain, global::Orleans.CodeGeneration.InvokeMethodRequest request)
        {
            global::System.Int32 interfaceId = request.InterfaceId;
            global::System.Int32 methodId = request.MethodId;
            global::System.Object[] arguments = request.Arguments;
            if (grain == null)
                throw new global::System.ArgumentNullException(@"grain");
            switch (interfaceId)
            {
                case 271477613:
                    switch (methodId)
                    {
                        case 458662693:
                            await ((global::Maddalena.Client.Interfaces.INewsGrain)grain).Create((global::Maddalena.Client.News)arguments[0]);
                            return null;
                        case 2070916615:
                            return await ((global::Maddalena.Client.Interfaces.INewsGrain)grain).GetNews();
                        case 1078770888:
                            return await ((global::Maddalena.Client.Interfaces.INewsGrain)grain).NewsInLabel((global::System.String)arguments[0], (global::Maddalena.Client.LabelValue)arguments[1]);
                        case 1336951808:
                            await ((global::Maddalena.Client.Interfaces.INewsGrain)grain).Update((global::Maddalena.Client.News)arguments[0]);
                            return null;
                        case 1088453568:
                            await ((global::Maddalena.Client.Interfaces.INewsGrain)grain).Delete((global::Maddalena.Client.News)arguments[0]);
                            return null;
                        default:
                            throw new global::System.NotImplementedException(@"interfaceId=" + 271477613 + @",methodId=" + methodId);
                    }

                default:
                    throw new global::System.NotImplementedException(@"interfaceId=" + interfaceId);
            }
        }

        public global::System.Int32 InterfaceId
        {
            get
            {
                return 271477613;
            }
        }

        public global::System.UInt16 InterfaceVersion
        {
            get
            {
                return 1;
            }
        }
    }
}

namespace OrleansGeneratedCodeD67B1861
{
    using global::Orleans;
    using global::System.Reflection;

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute(@"Orleans-CodeGenerator", @"2.0.0.0"), global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute, global::Orleans.CodeGeneration.SerializerAttribute(typeof (global::Maddalena.Client.Feed))]
    internal sealed class OrleansCodeGenMaddalena_Client_FeedSerializer
    {
        public OrleansCodeGenMaddalena_Client_FeedSerializer(global::Orleans.Serialization.IFieldUtils fieldUtils)
        {
        }

        [global::Orleans.CodeGeneration.CopierMethodAttribute]
        public global::System.Object DeepCopier(global::System.Object original, global::Orleans.Serialization.ICopyContext context)
        {
            global::Maddalena.Client.Feed input = ((global::Maddalena.Client.Feed)original);
            global::Maddalena.Client.Feed result = new global::Maddalena.Client.Feed();
            context.RecordCopy(original, result);
            result.Id = input.Id;
            result.LastCheck = input.LastCheck;
            result.Name = input.Name;
            result.Url = input.Url;
            return result;
        }

        [global::Orleans.CodeGeneration.SerializerMethodAttribute]
        public void Serializer(global::System.Object untypedInput, global::Orleans.Serialization.ISerializationContext context, global::System.Type expected)
        {
            global::Maddalena.Client.Feed input = (global::Maddalena.Client.Feed)untypedInput;
            context.SerializeInner(input.Id, typeof (global::System.String));
            context.SerializeInner(input.LastCheck, typeof (global::System.DateTime));
            context.SerializeInner(input.Name, typeof (global::System.String));
            context.SerializeInner(input.Url, typeof (global::System.String));
        }

        [global::Orleans.CodeGeneration.DeserializerMethodAttribute]
        public global::System.Object Deserializer(global::System.Type expected, global::Orleans.Serialization.IDeserializationContext context)
        {
            global::Maddalena.Client.Feed result = new global::Maddalena.Client.Feed();
            context.RecordObject(result);
            result.Id = (global::System.String)context.DeserializeInner(typeof (global::System.String));
            result.LastCheck = (global::System.DateTime)context.DeserializeInner(typeof (global::System.DateTime));
            result.Name = (global::System.String)context.DeserializeInner(typeof (global::System.String));
            result.Url = (global::System.String)context.DeserializeInner(typeof (global::System.String));
            return (global::Maddalena.Client.Feed)result;
        }
    }

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute(@"Orleans-CodeGenerator", @"2.0.0.0"), global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute, global::Orleans.CodeGeneration.SerializerAttribute(typeof (global::Maddalena.Client.News))]
    internal sealed class OrleansCodeGenMaddalena_Client_NewsSerializer
    {
        public OrleansCodeGenMaddalena_Client_NewsSerializer(global::Orleans.Serialization.IFieldUtils fieldUtils)
        {
        }

        [global::Orleans.CodeGeneration.CopierMethodAttribute]
        public global::System.Object DeepCopier(global::System.Object original, global::Orleans.Serialization.ICopyContext context)
        {
            global::Maddalena.Client.News input = ((global::Maddalena.Client.News)original);
            global::Maddalena.Client.News result = new global::Maddalena.Client.News();
            context.RecordCopy(original, result);
            result.Categories = (global::System.String[])context.DeepCopyInner(input.Categories);
            result.Description = input.Description;
            result.Id = input.Id;
            result.Link = input.Link;
            result.Timestamp = input.Timestamp;
            result.Title = input.Title;
            return result;
        }

        [global::Orleans.CodeGeneration.SerializerMethodAttribute]
        public void Serializer(global::System.Object untypedInput, global::Orleans.Serialization.ISerializationContext context, global::System.Type expected)
        {
            global::Maddalena.Client.News input = (global::Maddalena.Client.News)untypedInput;
            context.SerializeInner(input.Categories, typeof (global::System.String[]));
            context.SerializeInner(input.Description, typeof (global::System.String));
            context.SerializeInner(input.Id, typeof (global::System.String));
            context.SerializeInner(input.Link, typeof (global::System.String));
            context.SerializeInner(input.Timestamp, typeof (global::System.DateTime));
            context.SerializeInner(input.Title, typeof (global::System.String));
        }

        [global::Orleans.CodeGeneration.DeserializerMethodAttribute]
        public global::System.Object Deserializer(global::System.Type expected, global::Orleans.Serialization.IDeserializationContext context)
        {
            global::Maddalena.Client.News result = new global::Maddalena.Client.News();
            context.RecordObject(result);
            result.Categories = (global::System.String[])context.DeserializeInner(typeof (global::System.String[]));
            result.Description = (global::System.String)context.DeserializeInner(typeof (global::System.String));
            result.Id = (global::System.String)context.DeserializeInner(typeof (global::System.String));
            result.Link = (global::System.String)context.DeserializeInner(typeof (global::System.String));
            result.Timestamp = (global::System.DateTime)context.DeserializeInner(typeof (global::System.DateTime));
            result.Title = (global::System.String)context.DeserializeInner(typeof (global::System.String));
            return (global::Maddalena.Client.News)result;
        }
    }
}

namespace OrleansGeneratedCode
{
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute(@"Orleans-CodeGenerator", @"2.0.0.0")]
    internal sealed class OrleansCodeGenf819cada42FeaturePopulator : global::Orleans.Metadata.IFeaturePopulator<global::Orleans.Metadata.GrainInterfaceFeature>, global::Orleans.Metadata.IFeaturePopulator<global::Orleans.Metadata.GrainClassFeature>, global::Orleans.Metadata.IFeaturePopulator<global::Orleans.Serialization.SerializerFeature>
    {
        public void Populate(global::Orleans.Metadata.GrainInterfaceFeature feature)
        {
            feature.Interfaces.Add(new global::Orleans.Metadata.GrainInterfaceMetadata(typeof (global::Maddalena.Client.Interfaces.IFeedGrain), typeof (Maddalena.Client.Interfaces.OrleansCodeGenFeedGrainReference), typeof (Maddalena.Client.Interfaces.OrleansCodeGenFeedGrainMethodInvoker), -1642422289));
            feature.Interfaces.Add(new global::Orleans.Metadata.GrainInterfaceMetadata(typeof (global::Maddalena.Client.Interfaces.INewsGrain), typeof (Maddalena.Client.Interfaces.OrleansCodeGenNewsGrainReference), typeof (Maddalena.Client.Interfaces.OrleansCodeGenNewsGrainMethodInvoker), 271477613));
        }

        public void Populate(global::Orleans.Metadata.GrainClassFeature feature)
        {
        }

        public void Populate(global::Orleans.Serialization.SerializerFeature feature)
        {
            feature.AddSerializerType(typeof (global::Maddalena.Client.Feed), typeof (OrleansGeneratedCodeD67B1861.OrleansCodeGenMaddalena_Client_FeedSerializer));
            feature.AddSerializerType(typeof (global::Maddalena.Client.News), typeof (OrleansGeneratedCodeD67B1861.OrleansCodeGenMaddalena_Client_NewsSerializer));
            feature.AddKnownType(@"Maddalena.Client.ClusterClient,Maddalena.Client", @"Maddalena.Client.ClusterClient");
            feature.AddKnownType(@"Maddalena.Client.Feed,Maddalena.Client", @"Maddalena.Client.Feed");
            feature.AddKnownType(@"Maddalena.Client.LabelValue,Maddalena.Client", @"Maddalena.Client.LabelValue");
            feature.AddKnownType(@"Maddalena.Client.News,Maddalena.Client", @"Maddalena.Client.News");
            feature.AddKnownType(@"Maddalena.Client.Interfaces.IFeedGrain,Maddalena.Client", @"Maddalena.Client.Interfaces.IFeedGrain");
            feature.AddKnownType(@"Maddalena.Client.Interfaces.INewsGrain,Maddalena.Client", @"Maddalena.Client.Interfaces.INewsGrain");
            feature.AddKnownType(@"Microsoft.Extensions.Logging.ConsoleLoggerExtensions,Microsoft.Extensions.Logging.Console", @"Microsoft.Extensions.Logging.ConsoleLoggerExtensions");
            feature.AddKnownType(@"Microsoft.Extensions.Logging.Console.ConfigurationConsoleLoggerSettings,Microsoft.Extensions.Logging.Console", @"Microsoft.Extensions.Logging.Console.ConfigurationConsoleLoggerSettings");
            feature.AddKnownType(@"Microsoft.Extensions.Logging.Console.IConsoleLoggerSettings,Microsoft.Extensions.Logging.Console", @"Microsoft.Extensions.Logging.Console.IConsoleLoggerSettings");
            feature.AddKnownType(@"Microsoft.Extensions.Logging.Console.ConsoleLogger,Microsoft.Extensions.Logging.Console", @"Microsoft.Extensions.Logging.Console.ConsoleLogger");
            feature.AddKnownType(@"Microsoft.Extensions.Logging.Console.ConsoleLoggerOptions,Microsoft.Extensions.Logging.Console", @"Microsoft.Extensions.Logging.Console.ConsoleLoggerOptions");
            feature.AddKnownType(@"Microsoft.Extensions.Logging.Console.ConsoleLoggerProvider,Microsoft.Extensions.Logging.Console", @"Microsoft.Extensions.Logging.Console.ConsoleLoggerProvider");
            feature.AddKnownType(@"Microsoft.Extensions.Logging.Console.ConsoleLoggerSettings,Microsoft.Extensions.Logging.Console", @"Microsoft.Extensions.Logging.Console.ConsoleLoggerSettings");
            feature.AddKnownType(@"Microsoft.Extensions.Logging.Console.ConsoleLogScope,Microsoft.Extensions.Logging.Console", @"Microsoft.Extensions.Logging.Console.ConsoleLogScope");
            feature.AddKnownType(@"Microsoft.Extensions.Logging.Console.Internal.AnsiLogConsole,Microsoft.Extensions.Logging.Console", @"Microsoft.Extensions.Logging.Console.Internal.AnsiLogConsole");
            feature.AddKnownType(@"Microsoft.Extensions.Logging.Console.Internal.IConsole,Microsoft.Extensions.Logging.Console", @"Microsoft.Extensions.Logging.Console.Internal.IConsole");
            feature.AddKnownType(@"Microsoft.Extensions.Logging.Console.Internal.ConsoleLoggerProcessor,Microsoft.Extensions.Logging.Console", @"Microsoft.Extensions.Logging.Console.Internal.ConsoleLoggerProcessor");
            feature.AddKnownType(@"Microsoft.Extensions.Logging.Console.Internal.IAnsiSystemConsole,Microsoft.Extensions.Logging.Console", @"Microsoft.Extensions.Logging.Console.Internal.IAnsiSystemConsole");
            feature.AddKnownType(@"Microsoft.Extensions.Logging.Console.Internal.LogMessageEntry,Microsoft.Extensions.Logging.Console", @"Microsoft.Extensions.Logging.Console.Internal.LogMessageEntry");
            feature.AddKnownType(@"Microsoft.Extensions.Logging.Console.Internal.WindowsLogConsole,Microsoft.Extensions.Logging.Console", @"Microsoft.Extensions.Logging.Console.Internal.WindowsLogConsole");
        }
    }
}
#pragma warning restore 162
#pragma warning restore 219
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 649
#pragma warning restore 693
#pragma warning restore 1591
#pragma warning restore 1998
#endif