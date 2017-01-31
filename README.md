# Code.Testing
Inter application test enabler library

The project has the objective to provide the bases for the creation of applciation tests that requires remote mocked services.

In particular the project provides a collection of base classes to create and propagate a test context to a remote service and a base class for a WCF service that manages test sessions.
In case the mocked service are produced from a java hosted service wsdl which do not adheres completely to SOAP 1.1 specification, is available a WCF Dispatcher that allows the functioning of the mocked service even without SOAPActions (Action="" in the _svcutil_ produced skeleton).

The library is based on [CodeProbe.ExecutionControl](https://github.com/aliaslab-1984/CodeProbe) for the distribution of the _test context_. Hence both the client unittest application and the remote mocked service and even every intermediary between them must have correctly configured their endpoints in order to exploit the ContextSpan functionalities.

## Usage

In order to use the library is necessary:

1. In the mock project create a service that inherits from **TestSessionManagerServiceBase** which implements the contract:
<pre>
    [ServiceContract]
    public interface ITestSessionManagerService
    {
        [OperationContract]
        string Initialize();

        [OperationContract]
        void CleanUp(string sessionId);
    }
</pre>
which will allow to create and clean new separated test sessions for the concurrent use of the mocked service.

2. Create a library for the specific service (see *implementation* below).
3. Create some tests using the static methods exposed by the factory:

Client:

    using (((TestContextBuilder)TestContextFactory.GetBuilder()).WithFailure("XYZ01","Boh!!!!")
        .ForSession("test")
        .WithBoxId("05")
        .Build())
    {
         ...
    }

Server:

    using (TestContextFactory.Attach())
    {
       TestContextFactory.Current.WithSuccess....
       ....
    }

**NOTE**: the method _TestContextFactoryBase.Attach()_ performs the controls realtive to the success, exception and delay already,in particular:

- delay the execution of the bodyof the service of _ITestContext.ResponseDelayMillis_ milliseconds
- throws an exception of type _ITestContext.Exception_ or of type _System.Exception_ with the _Message_ property set with the creation error of the requested type.

## Implementation

In order to use the library it is necessary to create a test library for the desired tests and install that nuget package.

In the library must be created an implementation of *TestContextFactoryBase*, *TestContextBuilderBase*, *TestContextBase*.

### TestContextBase

Reresents a test context used on the mocked remote service to affect its execution. It implements the interface:

    public interface ITestContext : IDisposable
    {
        T GetValue<T>(string key, T defaultValue);

        string SessionId { get; }
        int ResponseDelayMillis { get; }
        bool WithSuccess { get; }
        string ResultCode { get; }
        string ResultMessage { get; }
        Exception Exception { get; }
    }

The context allows to get the information like the test session id, the delay of execution, the conditioned response (success or failure or with exception).
The method _GetValue_ has the purpose f making the context extensible in specific test libraries, by the use of extension methods:

    public static string CertificateType(this ITestContext ext)
    {
        return GetValue<string>(TestContextBuilder.CERT_TYPE);
    }

### TestContextBuilderBase

Represents the context builder, used in unittest methods to configure the behavior of the mocked remote method. It implements the interface: 
<pre>
    public interface ITestContextBuilder
    {
        ITestContextBuilder SetValue<T>(string key, T value);

        ITestContextBuilder ForSession(string sessionId);
        ITestContextBuilder WithDelayedResponse(int delayMillis);
        ITestContextBuilder WithSuccess();
        ITestContextBuilder WithFailure(string errorCode, string errorMessage);
        ITestContextBuilder WithException<T>() where T : Exception;

        ITestContext Build();
    }
</pre>
### TestContextFactoryBase

Represents the factory of the context e context builder and contains utility interfaces for the easy use of the library. For the implementation it is necessary:

- Create a _new_ implementation of the property *Instance* in such a way that will return the correct type of factory setting the field __instance_:

<pre>
	public new static TestContextFactoryBase Instance
    {
        get
        {
            if (TestContextFactoryBase._instance == null)
                TestContextFactoryBase._instance = new TestContextFactory();
            return TestContextFactoryBase._instance;
        }
    }
</pre>

- implement the property *TestContextPrefix* which is used to disambiguate calls from different tests server side.
- implementthe methods *GetBuilderImpl* and *AttachImpl* in such a way that will return the correct type of context and builder:
<pre>
    public override ITestContextBuilder GetBuilderImpl()
    {
        return new TestContextBuilder();
    }

    public override ITestContext AttachImpl()
    {
        if (Current == null)
            GetBuilder().Build();
        return base.AttachImpl();
    }
</pre>