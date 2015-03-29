using Org.Coos.Messaging.Impl;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Org.Coos.Messaging;
using System.Collections;

namespace TestProject1
{
    
    
    /// <summary>
    ///This is a test class for DefaultEndpointTest and is intended
    ///to contain all DefaultEndpointTest Unit Tests
    ///</summary>
    [TestClass()]
    public class DefaultEndpointTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


internal virtual DefaultEndpoint_Accessor CreateDefaultEndpoint_Accessor()
{
    // TODO: Instantiate an appropriate concrete class.
    DefaultEndpoint_Accessor target = null;
    return target;
}

/// <summary>
///A test for updateAliases
///</summary>
[TestMethod()]
[DeploymentItem("COOSPluginNET.dll")]
public void updateAliasesTest()
{
PrivateObject param0 = null; // TODO: Initialize to an appropriate value
DefaultEndpoint_Accessor target = new DefaultEndpoint_Accessor(param0); // TODO: Initialize to an appropriate value
    target.updateAliases();
    Assert.Inconclusive("A method that does not return a value cannot be verified.");
}

internal virtual DefaultEndpoint CreateDefaultEndpoint()
{
    // TODO: Instantiate an appropriate concrete class.
    DefaultEndpoint target = null;
    return target;
}

/// <summary>
///A test for unsubscribe
///</summary>
[TestMethod()]
public void unsubscribeTest()
{
DefaultEndpoint target = CreateDefaultEndpoint(); // TODO: Initialize to an appropriate value
    target.unsubscribe();
    Assert.Inconclusive("A method that does not return a value cannot be verified.");
}

/// <summary>
///A test for unsubscribe
///</summary>
[TestMethod()]
public void unsubscribeTest1()
{
DefaultEndpoint target = CreateDefaultEndpoint(); // TODO: Initialize to an appropriate value
ISubscriptionFilter filter = null; // TODO: Initialize to an appropriate value
    target.unsubscribe(filter);
    Assert.Inconclusive("A method that does not return a value cannot be verified.");
}

/// <summary>
///A test for unRegisterLCM
///</summary>
[TestMethod()]
[DeploymentItem("COOSPluginNET.dll")]
public void unRegisterLCMTest()
{
PrivateObject param0 = null; // TODO: Initialize to an appropriate value
DefaultEndpoint_Accessor target = new DefaultEndpoint_Accessor(param0); // TODO: Initialize to an appropriate value
    target.unRegisterLCM();
    Assert.Inconclusive("A method that does not return a value cannot be verified.");
}

/// <summary>
///A test for subscribe
///</summary>
[TestMethod()]
public void subscribeTest()
{
DefaultEndpoint target = CreateDefaultEndpoint(); // TODO: Initialize to an appropriate value
ISubscriptionFilter filter = null; // TODO: Initialize to an appropriate value
bool expected = false; // TODO: Initialize to an appropriate value
    bool actual;
    actual = target.subscribe(filter);
    Assert.AreEqual(expected, actual);
    Assert.Inconclusive("Verify the correctness of this test method.");
}

/// <summary>
///A test for stop
///</summary>
[TestMethod()]
public void stopTest()
{
DefaultEndpoint target = CreateDefaultEndpoint(); // TODO: Initialize to an appropriate value
    target.stop();
    Assert.Inconclusive("A method that does not return a value cannot be verified.");
}

/// <summary>
///A test for startLCMHeartbeat
///</summary>
[TestMethod()]
[DeploymentItem("COOSPluginNET.dll")]
public void startLCMHeartbeatTest()
{
PrivateObject param0 = null; // TODO: Initialize to an appropriate value
DefaultEndpoint_Accessor target = new DefaultEndpoint_Accessor(param0); // TODO: Initialize to an appropriate value
    target.startLCMHeartbeat();
    Assert.Inconclusive("A method that does not return a value cannot be verified.");
}

/// <summary>
///A test for start
///</summary>
[TestMethod()]
public void startTest()
{
DefaultEndpoint target = CreateDefaultEndpoint(); // TODO: Initialize to an appropriate value
    target.start();
    Assert.Inconclusive("A method that does not return a value cannot be verified.");
}

/// <summary>
///A test for shutDownEndpoint
///</summary>
[TestMethod()]
public void shutDownEndpointTest()
{
DefaultEndpoint target = CreateDefaultEndpoint(); // TODO: Initialize to an appropriate value
    target.shutDownEndpoint();
    Assert.Inconclusive("A method that does not return a value cannot be verified.");
}

/// <summary>
///A test for setTimeout
///</summary>
[TestMethod()]
public void setTimeoutTest()
{
DefaultEndpoint target = CreateDefaultEndpoint(); // TODO: Initialize to an appropriate value
long timeout = 0; // TODO: Initialize to an appropriate value
    target.setTimeout(timeout);
    Assert.Inconclusive("A method that does not return a value cannot be verified.");
}

/// <summary>
///A test for setProperties
///</summary>
[TestMethod()]
public void setPropertiesTest()
{
DefaultEndpoint target = CreateDefaultEndpoint(); // TODO: Initialize to an appropriate value
Hashtable properties = null; // TODO: Initialize to an appropriate value
    target.setProperties(properties);
    Assert.Inconclusive("A method that does not return a value cannot be verified.");
}

/// <summary>
///A test for setPlugin
///</summary>
[TestMethod()]
public void setPluginTest()
{
DefaultEndpoint target = CreateDefaultEndpoint(); // TODO: Initialize to an appropriate value
Plugin plugin = null; // TODO: Initialize to an appropriate value
    target.setPlugin(plugin);
    Assert.Inconclusive("A method that does not return a value cannot be verified.");
}

/// <summary>
///A test for setName
///</summary>
[TestMethod()]
public void setNameTest()
{
DefaultEndpoint target = CreateDefaultEndpoint(); // TODO: Initialize to an appropriate value
string name = string.Empty; // TODO: Initialize to an appropriate value
    target.setName(name);
    Assert.Inconclusive("A method that does not return a value cannot be verified.");
}

/// <summary>
///A test for setMaxPoolSize
///</summary>
[TestMethod()]
public void setMaxPoolSizeTest()
{
DefaultEndpoint target = CreateDefaultEndpoint(); // TODO: Initialize to an appropriate value
int maxPoolSize = 0; // TODO: Initialize to an appropriate value
    target.setMaxPoolSize(maxPoolSize);
    Assert.Inconclusive("A method that does not return a value cannot be verified.");
}

/// <summary>
///A test for setLinkAliases
///</summary>
[TestMethod()]
public void setLinkAliasesTest()
{
DefaultEndpoint target = CreateDefaultEndpoint(); // TODO: Initialize to an appropriate value
ArrayList regAliases = null; // TODO: Initialize to an appropriate value
Link outlink = null; // TODO: Initialize to an appropriate value
    target.setLinkAliases(regAliases, outlink);
    Assert.Inconclusive("A method that does not return a value cannot be verified.");
}

/// <summary>
///A test for setEndpointUuid
///</summary>
[TestMethod()]
public void setEndpointUuidTest()
{
DefaultEndpoint target = CreateDefaultEndpoint(); // TODO: Initialize to an appropriate value
string endpointUuid = string.Empty; // TODO: Initialize to an appropriate value
    target.setEndpointUuid(endpointUuid);
    Assert.Inconclusive("A method that does not return a value cannot be verified.");
}

/// <summary>
///A test for setEndpointUri
///</summary>
[TestMethod()]
public void setEndpointUriTest()
{
DefaultEndpoint target = CreateDefaultEndpoint(); // TODO: Initialize to an appropriate value
string endpointUri = string.Empty; // TODO: Initialize to an appropriate value
    target.setEndpointUri(endpointUri);
    Assert.Inconclusive("A method that does not return a value cannot be verified.");
}

/// <summary>
///A test for setEndpointState
///</summary>
[TestMethod()]
public void setEndpointStateTest()
{
DefaultEndpoint target = CreateDefaultEndpoint(); // TODO: Initialize to an appropriate value
string endpointState = string.Empty; // TODO: Initialize to an appropriate value
    target.setEndpointState(endpointState);
    Assert.Inconclusive("A method that does not return a value cannot be verified.");
}

/// <summary>
///A test for setChildStates
///</summary>
[TestMethod()]
public void setChildStatesTest()
{
DefaultEndpoint target = CreateDefaultEndpoint(); // TODO: Initialize to an appropriate value
Hashtable childStates = null; // TODO: Initialize to an appropriate value
    target.setChildStates(childStates);
    Assert.Inconclusive("A method that does not return a value cannot be verified.");
}

/// <summary>
///A test for setChildEndpointState
///</summary>
[TestMethod()]
public void setChildEndpointStateTest()
{
DefaultEndpoint target = CreateDefaultEndpoint(); // TODO: Initialize to an appropriate value
string childName = string.Empty; // TODO: Initialize to an appropriate value
string state = string.Empty; // TODO: Initialize to an appropriate value
    target.setChildEndpointState(childName, state);
    Assert.Inconclusive("A method that does not return a value cannot be verified.");
}

/// <summary>
///A test for sendMessage
///</summary>
[TestMethod()]
public void sendMessageTest()
{
DefaultEndpoint target = CreateDefaultEndpoint(); // TODO: Initialize to an appropriate value
IMessage msg = null; // TODO: Initialize to an appropriate value
string receiver = string.Empty; // TODO: Initialize to an appropriate value
string exchangePattern = string.Empty; // TODO: Initialize to an appropriate value
IMessage expected = null; // TODO: Initialize to an appropriate value
    IMessage actual;
    actual = target.sendMessage(msg, receiver, exchangePattern);
    Assert.AreEqual(expected, actual);
    Assert.Inconclusive("Verify the correctness of this test method.");
}

/// <summary>
///A test for resolveOutgoingProcessor
///</summary>
[TestMethod()]
[DeploymentItem("COOSPluginNET.dll")]
public void resolveOutgoingProcessorTest()
{
PrivateObject param0 = null; // TODO: Initialize to an appropriate value
DefaultEndpoint_Accessor target = new DefaultEndpoint_Accessor(param0); // TODO: Initialize to an appropriate value
string protocol = string.Empty; // TODO: Initialize to an appropriate value
IProcessor expected = null; // TODO: Initialize to an appropriate value
    IProcessor actual;
    actual = target.resolveOutgoingProcessor(protocol);
    Assert.AreEqual(expected, actual);
    Assert.Inconclusive("Verify the correctness of this test method.");
}

/// <summary>
///A test for reportState
///</summary>
[TestMethod()]
public void reportStateTest()
{
DefaultEndpoint target = CreateDefaultEndpoint(); // TODO: Initialize to an appropriate value
    target.reportState();
    Assert.Inconclusive("A method that does not return a value cannot be verified.");
}

/// <summary>
///A test for reportChildren
///</summary>
[TestMethod()]
[DeploymentItem("COOSPluginNET.dll")]
public void reportChildrenTest()
{
PrivateObject param0 = null; // TODO: Initialize to an appropriate value
DefaultEndpoint_Accessor target = new DefaultEndpoint_Accessor(param0); // TODO: Initialize to an appropriate value
    target.reportChildren();
    Assert.Inconclusive("A method that does not return a value cannot be verified.");
}

/// <summary>
///A test for reportChildState
///</summary>
[TestMethod()]
public void reportChildStateTest()
{
DefaultEndpoint target = CreateDefaultEndpoint(); // TODO: Initialize to an appropriate value
string childName = string.Empty; // TODO: Initialize to an appropriate value
    target.reportChildState(childName);
    Assert.Inconclusive("A method that does not return a value cannot be verified.");
}

/// <summary>
///A test for removeLinkById
///</summary>
[TestMethod()]
public void removeLinkByIdTest()
{
DefaultEndpoint target = CreateDefaultEndpoint(); // TODO: Initialize to an appropriate value
string linkId = string.Empty; // TODO: Initialize to an appropriate value
    target.removeLinkById(linkId);
    Assert.Inconclusive("A method that does not return a value cannot be verified.");
}

/// <summary>
///A test for removeLink
///</summary>
[TestMethod()]
public void removeLinkTest()
{
DefaultEndpoint target = CreateDefaultEndpoint(); // TODO: Initialize to an appropriate value
string id = string.Empty; // TODO: Initialize to an appropriate value
    target.removeLink(id);
    Assert.Inconclusive("A method that does not return a value cannot be verified.");
}

/// <summary>
///A test for removeAlias
///</summary>
[TestMethod()]
public void removeAliasTest()
{
DefaultEndpoint target = CreateDefaultEndpoint(); // TODO: Initialize to an appropriate value
string alias = string.Empty; // TODO: Initialize to an appropriate value
    target.removeAlias(alias);
    Assert.Inconclusive("A method that does not return a value cannot be verified.");
}

/// <summary>
///A test for registerLCM
///</summary>
[TestMethod()]
[DeploymentItem("COOSPluginNET.dll")]
public void registerLCMTest()
{
PrivateObject param0 = null; // TODO: Initialize to an appropriate value
DefaultEndpoint_Accessor target = new DefaultEndpoint_Accessor(param0); // TODO: Initialize to an appropriate value
    target.registerLCM();
    Assert.Inconclusive("A method that does not return a value cannot be verified.");
}

/// <summary>
///A test for publish
///</summary>
[TestMethod()]
public void publishTest()
{
DefaultEndpoint target = CreateDefaultEndpoint(); // TODO: Initialize to an appropriate value
INotification notification = null; // TODO: Initialize to an appropriate value
    target.publish(notification);
    Assert.Inconclusive("A method that does not return a value cannot be verified.");
}

/// <summary>
///A test for processMessage
///</summary>
[TestMethod()]
public void processMessageTest()
{
DefaultEndpoint target = CreateDefaultEndpoint(); // TODO: Initialize to an appropriate value
IMessage msg = null; // TODO: Initialize to an appropriate value
    target.processMessage(msg);
    Assert.Inconclusive("A method that does not return a value cannot be verified.");
}

/// <summary>
///A test for processExchange
///</summary>
[TestMethod()]
public void processExchangeTest()
{
DefaultEndpoint target = CreateDefaultEndpoint(); // TODO: Initialize to an appropriate value
IExchange exchange = null; // TODO: Initialize to an appropriate value
IAsyncCallback callback = null; // TODO: Initialize to an appropriate value
    target.processExchange(exchange, callback);
    Assert.Inconclusive("A method that does not return a value cannot be verified.");
}

/// <summary>
///A test for processExchange
///</summary>
[TestMethod()]
public void processExchangeTest1()
{
DefaultEndpoint target = CreateDefaultEndpoint(); // TODO: Initialize to an appropriate value
IExchange exchange = null; // TODO: Initialize to an appropriate value
IExchange expected = null; // TODO: Initialize to an appropriate value
    IExchange actual;
    actual = target.processExchange(exchange);
    Assert.AreEqual(expected, actual);
    Assert.Inconclusive("Verify the correctness of this test method.");
}

/// <summary>
///A test for prepareExchange
///</summary>
[TestMethod()]
[DeploymentItem("COOSPluginNET.dll")]
public void prepareExchangeTest()
{
PrivateObject param0 = null; // TODO: Initialize to an appropriate value
DefaultEndpoint_Accessor target = new DefaultEndpoint_Accessor(param0); // TODO: Initialize to an appropriate value
IExchange exchange = null; // TODO: Initialize to an appropriate value
IProcessor expected = null; // TODO: Initialize to an appropriate value
    IProcessor actual;
    actual = target.prepareExchange(exchange);
    Assert.AreEqual(expected, actual);
    Assert.Inconclusive("Verify the correctness of this test method.");
}

/// <summary>
///A test for preStart
///</summary>
[TestMethod()]
[DeploymentItem("COOSPluginNET.dll")]
public void preStartTest()
{
PrivateObject param0 = null; // TODO: Initialize to an appropriate value
DefaultEndpoint_Accessor target = new DefaultEndpoint_Accessor(param0); // TODO: Initialize to an appropriate value
    target.preStart();
    Assert.Inconclusive("A method that does not return a value cannot be verified.");
}

/// <summary>
///A test for postStart
///</summary>
[TestMethod()]
[DeploymentItem("COOSPluginNET.dll")]
public void postStartTest()
{
PrivateObject param0 = null; // TODO: Initialize to an appropriate value
DefaultEndpoint_Accessor target = new DefaultEndpoint_Accessor(param0); // TODO: Initialize to an appropriate value
    target.postStart();
    Assert.Inconclusive("A method that does not return a value cannot be verified.");
}

/// <summary>
///A test for isStateRunning
///</summary>
[TestMethod()]
[DeploymentItem("COOSPluginNET.dll")]
public void isStateRunningTest()
{
PrivateObject param0 = null; // TODO: Initialize to an appropriate value
DefaultEndpoint_Accessor target = new DefaultEndpoint_Accessor(param0); // TODO: Initialize to an appropriate value
bool expected = false; // TODO: Initialize to an appropriate value
    bool actual;
    actual = target.isStateRunning();
    Assert.AreEqual(expected, actual);
    Assert.Inconclusive("Verify the correctness of this test method.");
}

/// <summary>
///A test for initializeEndpoint
///</summary>
[TestMethod()]
public void initializeEndpointTest()
{
DefaultEndpoint target = CreateDefaultEndpoint(); // TODO: Initialize to an appropriate value
    target.initializeEndpoint();
    Assert.Inconclusive("A method that does not return a value cannot be verified.");
}

/// <summary>
///A test for hearbeatTask
///</summary>
[TestMethod()]
[DeploymentItem("COOSPluginNET.dll")]
public void hearbeatTaskTest()
{
PrivateObject param0 = null; // TODO: Initialize to an appropriate value
DefaultEndpoint_Accessor target = new DefaultEndpoint_Accessor(param0); // TODO: Initialize to an appropriate value
object state = null; // TODO: Initialize to an appropriate value
    target.hearbeatTask(state);
    Assert.Inconclusive("A method that does not return a value cannot be verified.");
}

/// <summary>
///A test for getServices
///</summary>
[TestMethod()]
public void getServicesTest()
{
DefaultEndpoint target = CreateDefaultEndpoint(); // TODO: Initialize to an appropriate value
ArrayList expected = null; // TODO: Initialize to an appropriate value
    ArrayList actual;
    actual = target.getServices();
    Assert.AreEqual(expected, actual);
    Assert.Inconclusive("Verify the correctness of this test method.");
}

/// <summary>
///A test for getProperties
///</summary>
[TestMethod()]
public void getPropertiesTest()
{
DefaultEndpoint target = CreateDefaultEndpoint(); // TODO: Initialize to an appropriate value
Hashtable expected = null; // TODO: Initialize to an appropriate value
    Hashtable actual;
    actual = target.getProperties();
    Assert.AreEqual(expected, actual);
    Assert.Inconclusive("Verify the correctness of this test method.");
}

/// <summary>
///A test for getPlugin
///</summary>
[TestMethod()]
public void getPluginTest()
{
DefaultEndpoint target = CreateDefaultEndpoint(); // TODO: Initialize to an appropriate value
Plugin expected = null; // TODO: Initialize to an appropriate value
    Plugin actual;
    actual = target.getPlugin();
    Assert.AreEqual(expected, actual);
    Assert.Inconclusive("Verify the correctness of this test method.");
}

/// <summary>
///A test for getLink
///</summary>
[TestMethod()]
public void getLinkTest()
{
DefaultEndpoint target = CreateDefaultEndpoint(); // TODO: Initialize to an appropriate value
string id = string.Empty; // TODO: Initialize to an appropriate value
Link expected = null; // TODO: Initialize to an appropriate value
    Link actual;
    actual = target.getLink(id);
    Assert.AreEqual(expected, actual);
    Assert.Inconclusive("Verify the correctness of this test method.");
}

/// <summary>
///A test for getEndpointUuid
///</summary>
[TestMethod()]
public void getEndpointUuidTest()
{
DefaultEndpoint target = CreateDefaultEndpoint(); // TODO: Initialize to an appropriate value
string expected = string.Empty; // TODO: Initialize to an appropriate value
    string actual;
    actual = target.getEndpointUuid();
    Assert.AreEqual(expected, actual);
    Assert.Inconclusive("Verify the correctness of this test method.");
}

/// <summary>
///A test for getEndpointUri
///</summary>
[TestMethod()]
public void getEndpointUriTest()
{
DefaultEndpoint target = CreateDefaultEndpoint(); // TODO: Initialize to an appropriate value
string expected = string.Empty; // TODO: Initialize to an appropriate value
    string actual;
    actual = target.getEndpointUri();
    Assert.AreEqual(expected, actual);
    Assert.Inconclusive("Verify the correctness of this test method.");
}

/// <summary>
///A test for getEndpointState
///</summary>
[TestMethod()]
public void getEndpointStateTest()
{
DefaultEndpoint target = CreateDefaultEndpoint(); // TODO: Initialize to an appropriate value
string expected = string.Empty; // TODO: Initialize to an appropriate value
    string actual;
    actual = target.getEndpointState();
    Assert.AreEqual(expected, actual);
    Assert.Inconclusive("Verify the correctness of this test method.");
}

/// <summary>
///A test for getDefaultProcessor
///</summary>
[TestMethod()]
public void getDefaultProcessorTest()
{
DefaultEndpoint target = CreateDefaultEndpoint(); // TODO: Initialize to an appropriate value
IProcessor expected = null; // TODO: Initialize to an appropriate value
    IProcessor actual;
    actual = target.getDefaultProcessor();
    Assert.AreEqual(expected, actual);
    Assert.Inconclusive("Verify the correctness of this test method.");
}

/// <summary>
///A test for getChildStates
///</summary>
[TestMethod()]
public void getChildStatesTest()
{
DefaultEndpoint target = CreateDefaultEndpoint(); // TODO: Initialize to an appropriate value
Hashtable expected = null; // TODO: Initialize to an appropriate value
    Hashtable actual;
    actual = target.getChildStates();
    Assert.AreEqual(expected, actual);
    Assert.Inconclusive("Verify the correctness of this test method.");
}

/// <summary>
///A test for getChildEndpointState
///</summary>
[TestMethod()]
public void getChildEndpointStateTest()
{
DefaultEndpoint target = CreateDefaultEndpoint(); // TODO: Initialize to an appropriate value
string childName = string.Empty; // TODO: Initialize to an appropriate value
string expected = string.Empty; // TODO: Initialize to an appropriate value
    string actual;
    actual = target.getChildEndpointState(childName);
    Assert.AreEqual(expected, actual);
    Assert.Inconclusive("Verify the correctness of this test method.");
}

/// <summary>
///A test for getAliases
///</summary>
[TestMethod()]
public void getAliasesTest()
{
DefaultEndpoint target = CreateDefaultEndpoint(); // TODO: Initialize to an appropriate value
ArrayList expected = null; // TODO: Initialize to an appropriate value
    ArrayList actual;
    actual = target.getAliases();
    Assert.AreEqual(expected, actual);
    Assert.Inconclusive("Verify the correctness of this test method.");
}

/// <summary>
///A test for createUuid
///</summary>
[TestMethod()]
public void createUuidTest()
{
DefaultEndpoint target = CreateDefaultEndpoint(); // TODO: Initialize to an appropriate value
string expected = string.Empty; // TODO: Initialize to an appropriate value
    string actual;
    actual = target.createUuid();
    Assert.AreEqual(expected, actual);
    Assert.Inconclusive("Verify the correctness of this test method.");
}

/// <summary>
///A test for createProducer
///</summary>
[TestMethod()]
public void createProducerTest()
{
DefaultEndpoint target = CreateDefaultEndpoint(); // TODO: Initialize to an appropriate value
IProducer expected = null; // TODO: Initialize to an appropriate value
    IProducer actual;
    actual = target.createProducer();
    Assert.AreEqual(expected, actual);
    Assert.Inconclusive("Verify the correctness of this test method.");
}

/// <summary>
///A test for createExchange
///</summary>
[TestMethod()]
public void createExchangeTest()
{
DefaultEndpoint target = CreateDefaultEndpoint(); // TODO: Initialize to an appropriate value
ExchangePattern pattern = null; // TODO: Initialize to an appropriate value
IExchange expected = null; // TODO: Initialize to an appropriate value
    IExchange actual;
    actual = target.createExchange(pattern);
    Assert.AreEqual(expected, actual);
    Assert.Inconclusive("Verify the correctness of this test method.");
}

/// <summary>
///A test for createExchange
///</summary>
[TestMethod()]
public void createExchangeTest1()
{
DefaultEndpoint target = CreateDefaultEndpoint(); // TODO: Initialize to an appropriate value
IExchange expected = null; // TODO: Initialize to an appropriate value
    IExchange actual;
    actual = target.createExchange();
    Assert.AreEqual(expected, actual);
    Assert.Inconclusive("Verify the correctness of this test method.");
}

/// <summary>
///A test for createConsumer
///</summary>
[TestMethod()]
public void createConsumerTest()
{
DefaultEndpoint target = CreateDefaultEndpoint(); // TODO: Initialize to an appropriate value
IConsumer expected = null; // TODO: Initialize to an appropriate value
    IConsumer actual;
    actual = target.createConsumer();
    Assert.AreEqual(expected, actual);
    Assert.Inconclusive("Verify the correctness of this test method.");
}

/// <summary>
///A test for checkDefer
///</summary>
[TestMethod()]
[DeploymentItem("COOSPluginNET.dll")]
public void checkDeferTest()
{
PrivateObject param0 = null; // TODO: Initialize to an appropriate value
DefaultEndpoint_Accessor target = new DefaultEndpoint_Accessor(param0); // TODO: Initialize to an appropriate value
IMessage msg = null; // TODO: Initialize to an appropriate value
bool expected = false; // TODO: Initialize to an appropriate value
    bool actual;
    actual = target.checkDefer(msg);
    Assert.AreEqual(expected, actual);
    Assert.Inconclusive("Verify the correctness of this test method.");
}

/// <summary>
///A test for addLink
///</summary>
[TestMethod()]
public void addLinkTest()
{
DefaultEndpoint target = CreateDefaultEndpoint(); // TODO: Initialize to an appropriate value
string protocol = string.Empty; // TODO: Initialize to an appropriate value
Link link = null; // TODO: Initialize to an appropriate value
    target.addLink(protocol, link);
    Assert.Inconclusive("A method that does not return a value cannot be verified.");
}

/// <summary>
///A test for addAlias
///</summary>
[TestMethod()]
public void addAliasTest()
{
DefaultEndpoint target = CreateDefaultEndpoint(); // TODO: Initialize to an appropriate value
string alias = string.Empty; // TODO: Initialize to an appropriate value
    target.addAlias(alias);
    Assert.Inconclusive("A method that does not return a value cannot be verified.");
}

/// <summary>
///A test for <processMessage>b__4
///</summary>
[TestMethod()]
[DeploymentItem("COOSPluginNET.dll")]
public void <processMessage>b__4Test()
{
PrivateObject param0 = null; // TODO: Initialize to an appropriate value
DefaultEndpoint_Accessor target = new DefaultEndpoint_Accessor(param0); // TODO: Initialize to an appropriate value
    target.<processMessage>b__4();
    Assert.Inconclusive("A method that does not return a value cannot be verified.");
}

/// <summary>
///A test for <processMessage>b__2
///</summary>
[TestMethod()]
[DeploymentItem("COOSPluginNET.dll")]
public void <processMessage>b__2Test()
{
PrivateObject param0 = null; // TODO: Initialize to an appropriate value
DefaultEndpoint_Accessor target = new DefaultEndpoint_Accessor(param0); // TODO: Initialize to an appropriate value
    target.<processMessage>b__2();
    Assert.Inconclusive("A method that does not return a value cannot be verified.");
}

/// <summary>
///A test for <hearbeatTask>b__12
///</summary>
[TestMethod()]
[DeploymentItem("COOSPluginNET.dll")]
public void <hearbeatTask>b__12Test()
{
PrivateObject param0 = null; // TODO: Initialize to an appropriate value
DefaultEndpoint_Accessor target = new DefaultEndpoint_Accessor(param0); // TODO: Initialize to an appropriate value
    target.<hearbeatTask>b__12();
    Assert.Inconclusive("A method that does not return a value cannot be verified.");
}
    }
}
