<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="LoanStop.Services.Azure" generation="1" functional="0" release="0" Id="9dd64f5c-b68e-4b1e-b50b-e752784ca627" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="LoanStop.Services.AzureGroup" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="LoanStop.Services.WebApi:Endpoint1" protocol="http">
          <inToChannel>
            <lBChannelMoniker name="/LoanStop.Services.Azure/LoanStop.Services.AzureGroup/LB:LoanStop.Services.WebApi:Endpoint1" />
          </inToChannel>
        </inPort>
        <inPort name="LoanStop.Services.WebApi:Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput" protocol="tcp">
          <inToChannel>
            <lBChannelMoniker name="/LoanStop.Services.Azure/LoanStop.Services.AzureGroup/LB:LoanStop.Services.WebApi:Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput" />
          </inToChannel>
        </inPort>
      </componentports>
      <settings>
        <aCS name="Certificate|LoanStop.Services.WebApi:Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" defaultValue="">
          <maps>
            <mapMoniker name="/LoanStop.Services.Azure/LoanStop.Services.AzureGroup/MapCertificate|LoanStop.Services.WebApi:Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" />
          </maps>
        </aCS>
        <aCS name="Certificate|LoanStop.Services.WebApi:Microsoft.WindowsAzure.Plugins.RemoteDebugger.TransportValidation" defaultValue="">
          <maps>
            <mapMoniker name="/LoanStop.Services.Azure/LoanStop.Services.AzureGroup/MapCertificate|LoanStop.Services.WebApi:Microsoft.WindowsAzure.Plugins.RemoteDebugger.TransportValidation" />
          </maps>
        </aCS>
        <aCS name="LoanStop.Services.WebApi:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" defaultValue="">
          <maps>
            <mapMoniker name="/LoanStop.Services.Azure/LoanStop.Services.AzureGroup/MapLoanStop.Services.WebApi:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" />
          </maps>
        </aCS>
        <aCS name="LoanStop.Services.WebApi:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" defaultValue="">
          <maps>
            <mapMoniker name="/LoanStop.Services.Azure/LoanStop.Services.AzureGroup/MapLoanStop.Services.WebApi:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" />
          </maps>
        </aCS>
        <aCS name="LoanStop.Services.WebApi:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" defaultValue="">
          <maps>
            <mapMoniker name="/LoanStop.Services.Azure/LoanStop.Services.AzureGroup/MapLoanStop.Services.WebApi:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" />
          </maps>
        </aCS>
        <aCS name="LoanStop.Services.WebApi:Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" defaultValue="">
          <maps>
            <mapMoniker name="/LoanStop.Services.Azure/LoanStop.Services.AzureGroup/MapLoanStop.Services.WebApi:Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" />
          </maps>
        </aCS>
        <aCS name="LoanStop.Services.WebApi:Microsoft.WindowsAzure.Plugins.RemoteDebugger.ClientThumbprint" defaultValue="">
          <maps>
            <mapMoniker name="/LoanStop.Services.Azure/LoanStop.Services.AzureGroup/MapLoanStop.Services.WebApi:Microsoft.WindowsAzure.Plugins.RemoteDebugger.ClientThumbprint" />
          </maps>
        </aCS>
        <aCS name="LoanStop.Services.WebApi:Microsoft.WindowsAzure.Plugins.RemoteDebugger.Connector.Enabled" defaultValue="">
          <maps>
            <mapMoniker name="/LoanStop.Services.Azure/LoanStop.Services.AzureGroup/MapLoanStop.Services.WebApi:Microsoft.WindowsAzure.Plugins.RemoteDebugger.Connector.Enabled" />
          </maps>
        </aCS>
        <aCS name="LoanStop.Services.WebApi:Microsoft.WindowsAzure.Plugins.RemoteDebugger.Connector.Version" defaultValue="">
          <maps>
            <mapMoniker name="/LoanStop.Services.Azure/LoanStop.Services.AzureGroup/MapLoanStop.Services.WebApi:Microsoft.WindowsAzure.Plugins.RemoteDebugger.Connector.Version" />
          </maps>
        </aCS>
        <aCS name="LoanStop.Services.WebApi:Microsoft.WindowsAzure.Plugins.RemoteDebugger.ServerThumbprint" defaultValue="">
          <maps>
            <mapMoniker name="/LoanStop.Services.Azure/LoanStop.Services.AzureGroup/MapLoanStop.Services.WebApi:Microsoft.WindowsAzure.Plugins.RemoteDebugger.ServerThumbprint" />
          </maps>
        </aCS>
        <aCS name="LoanStop.Services.WebApi:Microsoft.WindowsAzure.Plugins.RemoteForwarder.Enabled" defaultValue="">
          <maps>
            <mapMoniker name="/LoanStop.Services.Azure/LoanStop.Services.AzureGroup/MapLoanStop.Services.WebApi:Microsoft.WindowsAzure.Plugins.RemoteForwarder.Enabled" />
          </maps>
        </aCS>
        <aCS name="LoanStop.Services.WebApiInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/LoanStop.Services.Azure/LoanStop.Services.AzureGroup/MapLoanStop.Services.WebApiInstances" />
          </maps>
        </aCS>
      </settings>
      <channels>
        <sFSwitchChannel name="IE:LoanStop.Services.WebApi:Microsoft.WindowsAzure.Plugins.RemoteDebugger.Connector">
          <toPorts>
            <inPortMoniker name="/LoanStop.Services.Azure/LoanStop.Services.AzureGroup/LoanStop.Services.WebApi/Microsoft.WindowsAzure.Plugins.RemoteDebugger.Connector" />
          </toPorts>
        </sFSwitchChannel>
        <sFSwitchChannel name="IE:LoanStop.Services.WebApi:Microsoft.WindowsAzure.Plugins.RemoteDebugger.FileUpload">
          <toPorts>
            <inPortMoniker name="/LoanStop.Services.Azure/LoanStop.Services.AzureGroup/LoanStop.Services.WebApi/Microsoft.WindowsAzure.Plugins.RemoteDebugger.FileUpload" />
          </toPorts>
        </sFSwitchChannel>
        <sFSwitchChannel name="IE:LoanStop.Services.WebApi:Microsoft.WindowsAzure.Plugins.RemoteDebugger.Forwarder">
          <toPorts>
            <inPortMoniker name="/LoanStop.Services.Azure/LoanStop.Services.AzureGroup/LoanStop.Services.WebApi/Microsoft.WindowsAzure.Plugins.RemoteDebugger.Forwarder" />
          </toPorts>
        </sFSwitchChannel>
        <lBChannel name="LB:LoanStop.Services.WebApi:Endpoint1">
          <toPorts>
            <inPortMoniker name="/LoanStop.Services.Azure/LoanStop.Services.AzureGroup/LoanStop.Services.WebApi/Endpoint1" />
          </toPorts>
        </lBChannel>
        <lBChannel name="LB:LoanStop.Services.WebApi:Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput">
          <toPorts>
            <inPortMoniker name="/LoanStop.Services.Azure/LoanStop.Services.AzureGroup/LoanStop.Services.WebApi/Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput" />
          </toPorts>
        </lBChannel>
        <sFSwitchChannel name="SW:LoanStop.Services.WebApi:Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp">
          <toPorts>
            <inPortMoniker name="/LoanStop.Services.Azure/LoanStop.Services.AzureGroup/LoanStop.Services.WebApi/Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp" />
          </toPorts>
        </sFSwitchChannel>
      </channels>
      <maps>
        <map name="MapCertificate|LoanStop.Services.WebApi:Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" kind="Identity">
          <certificate>
            <certificateMoniker name="/LoanStop.Services.Azure/LoanStop.Services.AzureGroup/LoanStop.Services.WebApi/Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" />
          </certificate>
        </map>
        <map name="MapCertificate|LoanStop.Services.WebApi:Microsoft.WindowsAzure.Plugins.RemoteDebugger.TransportValidation" kind="Identity">
          <certificate>
            <certificateMoniker name="/LoanStop.Services.Azure/LoanStop.Services.AzureGroup/LoanStop.Services.WebApi/Microsoft.WindowsAzure.Plugins.RemoteDebugger.TransportValidation" />
          </certificate>
        </map>
        <map name="MapLoanStop.Services.WebApi:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" kind="Identity">
          <setting>
            <aCSMoniker name="/LoanStop.Services.Azure/LoanStop.Services.AzureGroup/LoanStop.Services.WebApi/Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" />
          </setting>
        </map>
        <map name="MapLoanStop.Services.WebApi:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" kind="Identity">
          <setting>
            <aCSMoniker name="/LoanStop.Services.Azure/LoanStop.Services.AzureGroup/LoanStop.Services.WebApi/Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" />
          </setting>
        </map>
        <map name="MapLoanStop.Services.WebApi:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" kind="Identity">
          <setting>
            <aCSMoniker name="/LoanStop.Services.Azure/LoanStop.Services.AzureGroup/LoanStop.Services.WebApi/Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" />
          </setting>
        </map>
        <map name="MapLoanStop.Services.WebApi:Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" kind="Identity">
          <setting>
            <aCSMoniker name="/LoanStop.Services.Azure/LoanStop.Services.AzureGroup/LoanStop.Services.WebApi/Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" />
          </setting>
        </map>
        <map name="MapLoanStop.Services.WebApi:Microsoft.WindowsAzure.Plugins.RemoteDebugger.ClientThumbprint" kind="Identity">
          <setting>
            <aCSMoniker name="/LoanStop.Services.Azure/LoanStop.Services.AzureGroup/LoanStop.Services.WebApi/Microsoft.WindowsAzure.Plugins.RemoteDebugger.ClientThumbprint" />
          </setting>
        </map>
        <map name="MapLoanStop.Services.WebApi:Microsoft.WindowsAzure.Plugins.RemoteDebugger.Connector.Enabled" kind="Identity">
          <setting>
            <aCSMoniker name="/LoanStop.Services.Azure/LoanStop.Services.AzureGroup/LoanStop.Services.WebApi/Microsoft.WindowsAzure.Plugins.RemoteDebugger.Connector.Enabled" />
          </setting>
        </map>
        <map name="MapLoanStop.Services.WebApi:Microsoft.WindowsAzure.Plugins.RemoteDebugger.Connector.Version" kind="Identity">
          <setting>
            <aCSMoniker name="/LoanStop.Services.Azure/LoanStop.Services.AzureGroup/LoanStop.Services.WebApi/Microsoft.WindowsAzure.Plugins.RemoteDebugger.Connector.Version" />
          </setting>
        </map>
        <map name="MapLoanStop.Services.WebApi:Microsoft.WindowsAzure.Plugins.RemoteDebugger.ServerThumbprint" kind="Identity">
          <setting>
            <aCSMoniker name="/LoanStop.Services.Azure/LoanStop.Services.AzureGroup/LoanStop.Services.WebApi/Microsoft.WindowsAzure.Plugins.RemoteDebugger.ServerThumbprint" />
          </setting>
        </map>
        <map name="MapLoanStop.Services.WebApi:Microsoft.WindowsAzure.Plugins.RemoteForwarder.Enabled" kind="Identity">
          <setting>
            <aCSMoniker name="/LoanStop.Services.Azure/LoanStop.Services.AzureGroup/LoanStop.Services.WebApi/Microsoft.WindowsAzure.Plugins.RemoteForwarder.Enabled" />
          </setting>
        </map>
        <map name="MapLoanStop.Services.WebApiInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/LoanStop.Services.Azure/LoanStop.Services.AzureGroup/LoanStop.Services.WebApiInstances" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="LoanStop.Services.WebApi" generation="1" functional="0" release="0" software="C:\GitHub\loanstop\Service\LoanStop.Services.Azure\csx\Debug\roles\LoanStop.Services.WebApi" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaIISHost.exe " memIndex="-1" hostingEnvironment="frontendadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="Endpoint1" protocol="http" portRanges="8080" />
              <inPort name="Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput" protocol="tcp" />
              <inPort name="Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp" protocol="tcp" portRanges="3389" />
              <outPort name="LoanStop.Services.WebApi:Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp" protocol="tcp">
                <outToChannel>
                  <sFSwitchChannelMoniker name="/LoanStop.Services.Azure/LoanStop.Services.AzureGroup/SW:LoanStop.Services.WebApi:Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp" />
                </outToChannel>
              </outPort>
            </componentports>
            <settings>
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteDebugger.ClientThumbprint" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteDebugger.Connector.Enabled" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteDebugger.Connector.Version" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteDebugger.ServerThumbprint" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteForwarder.Enabled" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;LoanStop.Services.WebApi&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;LoanStop.Services.WebApi&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.RemoteDebugger.Connector&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.RemoteDebugger.FileUpload&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.RemoteDebugger.Forwarder&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
            <storedcertificates>
              <storedCertificate name="Stored0Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" certificateStore="My" certificateLocation="System">
                <certificate>
                  <certificateMoniker name="/LoanStop.Services.Azure/LoanStop.Services.AzureGroup/LoanStop.Services.WebApi/Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" />
                </certificate>
              </storedCertificate>
              <storedCertificate name="Stored1Microsoft.WindowsAzure.Plugins.RemoteDebugger.TransportValidation" certificateStore="My" certificateLocation="System">
                <certificate>
                  <certificateMoniker name="/LoanStop.Services.Azure/LoanStop.Services.AzureGroup/LoanStop.Services.WebApi/Microsoft.WindowsAzure.Plugins.RemoteDebugger.TransportValidation" />
                </certificate>
              </storedCertificate>
            </storedcertificates>
            <certificates>
              <certificate name="Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" />
              <certificate name="Microsoft.WindowsAzure.Plugins.RemoteDebugger.TransportValidation" />
            </certificates>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/LoanStop.Services.Azure/LoanStop.Services.AzureGroup/LoanStop.Services.WebApiInstances" />
            <sCSPolicyUpdateDomainMoniker name="/LoanStop.Services.Azure/LoanStop.Services.AzureGroup/LoanStop.Services.WebApiUpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/LoanStop.Services.Azure/LoanStop.Services.AzureGroup/LoanStop.Services.WebApiFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyUpdateDomain name="LoanStop.Services.WebApiUpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyFaultDomain name="LoanStop.Services.WebApiFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyID name="LoanStop.Services.WebApiInstances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
  <implements>
    <implementation Id="d4ed0df6-f577-4b89-a0f8-0f5f38b6bc29" ref="Microsoft.RedDog.Contract\ServiceContract\LoanStop.Services.AzureContract@ServiceDefinition">
      <interfacereferences>
        <interfaceReference Id="d722a336-b1ed-4772-a36a-280f2a099b71" ref="Microsoft.RedDog.Contract\Interface\LoanStop.Services.WebApi:Endpoint1@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/LoanStop.Services.Azure/LoanStop.Services.AzureGroup/LoanStop.Services.WebApi:Endpoint1" />
          </inPort>
        </interfaceReference>
        <interfaceReference Id="f12a5962-8f0f-4bf2-a3c6-a4b630f02c3a" ref="Microsoft.RedDog.Contract\Interface\LoanStop.Services.WebApi:Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/LoanStop.Services.Azure/LoanStop.Services.AzureGroup/LoanStop.Services.WebApi:Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>