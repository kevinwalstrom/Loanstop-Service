﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18408
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.18408.
// 
#pragma warning disable 1591

namespace LoanStop.ACHWorks.com.achworks.securesoap {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="Iverbinding", Namespace="http://tempuri.org/")]
    public partial class Iverservice : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback ConnectionCheckOperationCompleted;
        
        private System.Threading.SendOrPostCallback TSSVerifyOperationCompleted;
        
        private System.Threading.SendOrPostCallback TSSVerifyBatchOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public Iverservice() {
            this.Url = global::LoanStop.ACHWorks.Properties.Settings.Default.LoanStop_ACHWorks_com_achworks_securesoap_Iverservice;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event ConnectionCheckCompletedEventHandler ConnectionCheckCompleted;
        
        /// <remarks/>
        public event TSSVerifyCompletedEventHandler TSSVerifyCompleted;
        
        /// <remarks/>
        public event TSSVerifyBatchCompletedEventHandler TSSVerifyBatchCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("urn:verIntf-Iver#ConnectionCheck", RequestNamespace="urn:verIntf-Iver", ResponseNamespace="urn:verIntf-Iver")]
        [return: System.Xml.Serialization.SoapElementAttribute("return")]
        public string ConnectionCheck(string LocID, string Company, string CompanyKey) {
            object[] results = this.Invoke("ConnectionCheck", new object[] {
                        LocID,
                        Company,
                        CompanyKey});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void ConnectionCheckAsync(string LocID, string Company, string CompanyKey) {
            this.ConnectionCheckAsync(LocID, Company, CompanyKey, null);
        }
        
        /// <remarks/>
        public void ConnectionCheckAsync(string LocID, string Company, string CompanyKey, object userState) {
            if ((this.ConnectionCheckOperationCompleted == null)) {
                this.ConnectionCheckOperationCompleted = new System.Threading.SendOrPostCallback(this.OnConnectionCheckOperationCompleted);
            }
            this.InvokeAsync("ConnectionCheck", new object[] {
                        LocID,
                        Company,
                        CompanyKey}, this.ConnectionCheckOperationCompleted, userState);
        }
        
        private void OnConnectionCheckOperationCompleted(object arg) {
            if ((this.ConnectionCheckCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ConnectionCheckCompleted(this, new ConnectionCheckCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("urn:verIntf-Iver#TSSVerify", RequestNamespace="urn:verIntf-Iver", ResponseNamespace="urn:verIntf-Iver")]
        [return: System.Xml.Serialization.SoapElementAttribute("return")]
        public ResultVer TSSVerify(string InpServiceCode, CompanyInfo InpCompanyInfo, CustomerInfo InpCustomerInfo, AcctInfo InpAcctInfo) {
            object[] results = this.Invoke("TSSVerify", new object[] {
                        InpServiceCode,
                        InpCompanyInfo,
                        InpCustomerInfo,
                        InpAcctInfo});
            return ((ResultVer)(results[0]));
        }
        
        /// <remarks/>
        public void TSSVerifyAsync(string InpServiceCode, CompanyInfo InpCompanyInfo, CustomerInfo InpCustomerInfo, AcctInfo InpAcctInfo) {
            this.TSSVerifyAsync(InpServiceCode, InpCompanyInfo, InpCustomerInfo, InpAcctInfo, null);
        }
        
        /// <remarks/>
        public void TSSVerifyAsync(string InpServiceCode, CompanyInfo InpCompanyInfo, CustomerInfo InpCustomerInfo, AcctInfo InpAcctInfo, object userState) {
            if ((this.TSSVerifyOperationCompleted == null)) {
                this.TSSVerifyOperationCompleted = new System.Threading.SendOrPostCallback(this.OnTSSVerifyOperationCompleted);
            }
            this.InvokeAsync("TSSVerify", new object[] {
                        InpServiceCode,
                        InpCompanyInfo,
                        InpCustomerInfo,
                        InpAcctInfo}, this.TSSVerifyOperationCompleted, userState);
        }
        
        private void OnTSSVerifyOperationCompleted(object arg) {
            if ((this.TSSVerifyCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.TSSVerifyCompleted(this, new TSSVerifyCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("urn:verIntf-Iver#TSSVerifyBatch", RequestNamespace="urn:verIntf-Iver", ResponseNamespace="urn:verIntf-Iver")]
        [return: System.Xml.Serialization.SoapElementAttribute("return")]
        public string[] TSSVerifyBatch(string InpServiceCode, CompanyInfo InpCompanyInfo, string[] InpBatchInput) {
            object[] results = this.Invoke("TSSVerifyBatch", new object[] {
                        InpServiceCode,
                        InpCompanyInfo,
                        InpBatchInput});
            return ((string[])(results[0]));
        }
        
        /// <remarks/>
        public void TSSVerifyBatchAsync(string InpServiceCode, CompanyInfo InpCompanyInfo, string[] InpBatchInput) {
            this.TSSVerifyBatchAsync(InpServiceCode, InpCompanyInfo, InpBatchInput, null);
        }
        
        /// <remarks/>
        public void TSSVerifyBatchAsync(string InpServiceCode, CompanyInfo InpCompanyInfo, string[] InpBatchInput, object userState) {
            if ((this.TSSVerifyBatchOperationCompleted == null)) {
                this.TSSVerifyBatchOperationCompleted = new System.Threading.SendOrPostCallback(this.OnTSSVerifyBatchOperationCompleted);
            }
            this.InvokeAsync("TSSVerifyBatch", new object[] {
                        InpServiceCode,
                        InpCompanyInfo,
                        InpBatchInput}, this.TSSVerifyBatchOperationCompleted, userState);
        }
        
        private void OnTSSVerifyBatchOperationCompleted(object arg) {
            if ((this.TSSVerifyBatchCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.TSSVerifyBatchCompleted(this, new TSSVerifyBatchCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.18408")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.SoapTypeAttribute(Namespace="urn:verIntf")]
    public partial class CompanyInfo {
        
        private string locIDField;
        
        private string companyField;
        
        private string companyKeyField;
        
        private string emailField;
        
        /// <remarks/>
        public string LocID {
            get {
                return this.locIDField;
            }
            set {
                this.locIDField = value;
            }
        }
        
        /// <remarks/>
        public string Company {
            get {
                return this.companyField;
            }
            set {
                this.companyField = value;
            }
        }
        
        /// <remarks/>
        public string CompanyKey {
            get {
                return this.companyKeyField;
            }
            set {
                this.companyKeyField = value;
            }
        }
        
        /// <remarks/>
        public string Email {
            get {
                return this.emailField;
            }
            set {
                this.emailField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.18408")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.SoapTypeAttribute(Namespace="urn:verIntf")]
    public partial class ResultVer {
        
        private string refNoField;
        
        private string requestCodeField;
        
        private string successCodeField;
        
        private string resultVerXMLField;
        
        private string cKVXMLField;
        
        private string sSNXMLField;
        
        private string bTRNXMLField;
        
        private string oFACXMLField;
        
        private string serverTimeField;
        
        private string remarksField;
        
        /// <remarks/>
        public string RefNo {
            get {
                return this.refNoField;
            }
            set {
                this.refNoField = value;
            }
        }
        
        /// <remarks/>
        public string RequestCode {
            get {
                return this.requestCodeField;
            }
            set {
                this.requestCodeField = value;
            }
        }
        
        /// <remarks/>
        public string SuccessCode {
            get {
                return this.successCodeField;
            }
            set {
                this.successCodeField = value;
            }
        }
        
        /// <remarks/>
        public string ResultVerXML {
            get {
                return this.resultVerXMLField;
            }
            set {
                this.resultVerXMLField = value;
            }
        }
        
        /// <remarks/>
        public string CKVXML {
            get {
                return this.cKVXMLField;
            }
            set {
                this.cKVXMLField = value;
            }
        }
        
        /// <remarks/>
        public string SSNXML {
            get {
                return this.sSNXMLField;
            }
            set {
                this.sSNXMLField = value;
            }
        }
        
        /// <remarks/>
        public string BTRNXML {
            get {
                return this.bTRNXMLField;
            }
            set {
                this.bTRNXMLField = value;
            }
        }
        
        /// <remarks/>
        public string OFACXML {
            get {
                return this.oFACXMLField;
            }
            set {
                this.oFACXMLField = value;
            }
        }
        
        /// <remarks/>
        public string ServerTime {
            get {
                return this.serverTimeField;
            }
            set {
                this.serverTimeField = value;
            }
        }
        
        /// <remarks/>
        public string Remarks {
            get {
                return this.remarksField;
            }
            set {
                this.remarksField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.18408")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.SoapTypeAttribute(Namespace="urn:verIntf")]
    public partial class AcctInfo {
        
        private string amountField;
        
        private string acctTypeField;
        
        private string acctNoField;
        
        private string routingNoField;
        
        /// <remarks/>
        public string Amount {
            get {
                return this.amountField;
            }
            set {
                this.amountField = value;
            }
        }
        
        /// <remarks/>
        public string AcctType {
            get {
                return this.acctTypeField;
            }
            set {
                this.acctTypeField = value;
            }
        }
        
        /// <remarks/>
        public string AcctNo {
            get {
                return this.acctNoField;
            }
            set {
                this.acctNoField = value;
            }
        }
        
        /// <remarks/>
        public string RoutingNo {
            get {
                return this.routingNoField;
            }
            set {
                this.routingNoField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.18408")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.SoapTypeAttribute(Namespace="urn:verIntf")]
    public partial class CustomerInfo {
        
        private string customerNameField;
        
        private string sSNField;
        
        private string dateOfBirthField;
        
        /// <remarks/>
        public string CustomerName {
            get {
                return this.customerNameField;
            }
            set {
                this.customerNameField = value;
            }
        }
        
        /// <remarks/>
        public string SSN {
            get {
                return this.sSNField;
            }
            set {
                this.sSNField = value;
            }
        }
        
        /// <remarks/>
        public string DateOfBirth {
            get {
                return this.dateOfBirthField;
            }
            set {
                this.dateOfBirthField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")]
    public delegate void ConnectionCheckCompletedEventHandler(object sender, ConnectionCheckCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ConnectionCheckCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal ConnectionCheckCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")]
    public delegate void TSSVerifyCompletedEventHandler(object sender, TSSVerifyCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class TSSVerifyCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal TSSVerifyCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public ResultVer Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((ResultVer)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")]
    public delegate void TSSVerifyBatchCompletedEventHandler(object sender, TSSVerifyBatchCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class TSSVerifyBatchCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal TSSVerifyBatchCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string[])(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591