﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.1
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace QRST_DI_MS_Console.RockService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://service.bp.zjugis.edu", ConfigurationName="RockService.RockServicePortType")]
    public interface RockServicePortType {
        
        // CODEGEN: 参数“return”需要其他方案信息，使用参数模式无法捕获这些信息。特定特性为“System.Xml.Serialization.XmlElementAttribute”。
        [System.ServiceModel.OperationContractAttribute(Action="urn:getSSLB", ReplyAction="urn:getSSLBResponse")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [return: System.ServiceModel.MessageParameterAttribute(Name="return")]
        QRST_DI_MS_Console.RockService.getSSLBResponse getSSLB(QRST_DI_MS_Console.RockService.getSSLBRequest request);
        
        // CODEGEN: 参数“return”需要其他方案信息，使用参数模式无法捕获这些信息。特定特性为“System.Xml.Serialization.XmlElementAttribute”。
        [System.ServiceModel.OperationContractAttribute(Action="urn:getGPSJ", ReplyAction="urn:getGPSJResponse")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [return: System.ServiceModel.MessageParameterAttribute(Name="return")]
        QRST_DI_MS_Console.RockService.getGPSJResponse getGPSJ(QRST_DI_MS_Console.RockService.getGPSJRequest request);
        
        // CODEGEN: 参数“return”需要其他方案信息，使用参数模式无法捕获这些信息。特定特性为“System.Xml.Serialization.XmlElementAttribute”。
        [System.ServiceModel.OperationContractAttribute(Action="urn:getTypes", ReplyAction="urn:getTypesResponse")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [return: System.ServiceModel.MessageParameterAttribute(Name="return")]
        QRST_DI_MS_Console.RockService.getTypesResponse getTypes(QRST_DI_MS_Console.RockService.getTypesRequest request);
        
        // CODEGEN: 参数“return”需要其他方案信息，使用参数模式无法捕获这些信息。特定特性为“System.Xml.Serialization.XmlElementAttribute”。
        [System.ServiceModel.OperationContractAttribute(Action="urn:getQueryRocks", ReplyAction="urn:getQueryRocksResponse")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [return: System.ServiceModel.MessageParameterAttribute(Name="return")]
        QRST_DI_MS_Console.RockService.getQueryRocksResponse getQueryRocks(QRST_DI_MS_Console.RockService.getQueryRocksRequest request);
        
        // CODEGEN: 参数“return”需要其他方案信息，使用参数模式无法捕获这些信息。特定特性为“System.Xml.Serialization.XmlElementAttribute”。
        [System.ServiceModel.OperationContractAttribute(Action="urn:getSubTypes", ReplyAction="urn:getSubTypesResponse")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [return: System.ServiceModel.MessageParameterAttribute(Name="return")]
        QRST_DI_MS_Console.RockService.getSubTypesResponse getSubTypes(QRST_DI_MS_Console.RockService.getSubTypesRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="getSSLB", WrapperNamespace="http://service.bp.zjugis.edu", IsWrapped=true)]
    public partial class getSSLBRequest {
        
        public getSSLBRequest() {
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="getSSLBResponse", WrapperNamespace="http://service.bp.zjugis.edu", IsWrapped=true)]
    public partial class getSSLBResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://service.bp.zjugis.edu", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string @return;
        
        public getSSLBResponse() {
        }
        
        public getSSLBResponse(string @return) {
            this.@return = @return;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="getGPSJ", WrapperNamespace="http://service.bp.zjugis.edu", IsWrapped=true)]
    public partial class getGPSJRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://service.bp.zjugis.edu", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string fseq;
        
        public getGPSJRequest() {
        }
        
        public getGPSJRequest(string fseq) {
            this.fseq = fseq;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="getGPSJResponse", WrapperNamespace="http://service.bp.zjugis.edu", IsWrapped=true)]
    public partial class getGPSJResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://service.bp.zjugis.edu", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string @return;
        
        public getGPSJResponse() {
        }
        
        public getGPSJResponse(string @return) {
            this.@return = @return;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="getTypes", WrapperNamespace="http://service.bp.zjugis.edu", IsWrapped=true)]
    public partial class getTypesRequest {
        
        public getTypesRequest() {
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="getTypesResponse", WrapperNamespace="http://service.bp.zjugis.edu", IsWrapped=true)]
    public partial class getTypesResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://service.bp.zjugis.edu", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string @return;
        
        public getTypesResponse() {
        }
        
        public getTypesResponse(string @return) {
            this.@return = @return;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="getQueryRocks", WrapperNamespace="http://service.bp.zjugis.edu", IsWrapped=true)]
    public partial class getQueryRocksRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://service.bp.zjugis.edu", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string dts;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://service.bp.zjugis.edu", Order=1)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string dte;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://service.bp.zjugis.edu", Order=2)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string maxLat;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://service.bp.zjugis.edu", Order=3)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string maxLng;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://service.bp.zjugis.edu", Order=4)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string minLat;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://service.bp.zjugis.edu", Order=5)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string minLng;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://service.bp.zjugis.edu", Order=6)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string ykmc;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://service.bp.zjugis.edu", Order=7)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string yklb;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://service.bp.zjugis.edu", Order=8)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string ykzl;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://service.bp.zjugis.edu", Order=9)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string sslb;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://service.bp.zjugis.edu", Order=10)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string pagenum;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://service.bp.zjugis.edu", Order=11)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string pagesize;
        
        public getQueryRocksRequest() {
        }
        
        public getQueryRocksRequest(string dts, string dte, string maxLat, string maxLng, string minLat, string minLng, string ykmc, string yklb, string ykzl, string sslb, string pagenum, string pagesize) {
            this.dts = dts;
            this.dte = dte;
            this.maxLat = maxLat;
            this.maxLng = maxLng;
            this.minLat = minLat;
            this.minLng = minLng;
            this.ykmc = ykmc;
            this.yklb = yklb;
            this.ykzl = ykzl;
            this.sslb = sslb;
            this.pagenum = pagenum;
            this.pagesize = pagesize;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="getQueryRocksResponse", WrapperNamespace="http://service.bp.zjugis.edu", IsWrapped=true)]
    public partial class getQueryRocksResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://service.bp.zjugis.edu", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string @return;
        
        public getQueryRocksResponse() {
        }
        
        public getQueryRocksResponse(string @return) {
            this.@return = @return;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="getSubTypes", WrapperNamespace="http://service.bp.zjugis.edu", IsWrapped=true)]
    public partial class getSubTypesRequest {
        
        public getSubTypesRequest() {
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="getSubTypesResponse", WrapperNamespace="http://service.bp.zjugis.edu", IsWrapped=true)]
    public partial class getSubTypesResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://service.bp.zjugis.edu", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string @return;
        
        public getSubTypesResponse() {
        }
        
        public getSubTypesResponse(string @return) {
            this.@return = @return;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface RockServicePortTypeChannel : QRST_DI_MS_Console.RockService.RockServicePortType, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class RockServicePortTypeClient : System.ServiceModel.ClientBase<QRST_DI_MS_Console.RockService.RockServicePortType>, QRST_DI_MS_Console.RockService.RockServicePortType {
        
        public RockServicePortTypeClient() {
        }
        
        public RockServicePortTypeClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public RockServicePortTypeClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public RockServicePortTypeClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public RockServicePortTypeClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        QRST_DI_MS_Console.RockService.getSSLBResponse QRST_DI_MS_Console.RockService.RockServicePortType.getSSLB(QRST_DI_MS_Console.RockService.getSSLBRequest request) {
            return base.Channel.getSSLB(request);
        }
        
        public string getSSLB() {
            QRST_DI_MS_Console.RockService.getSSLBRequest inValue = new QRST_DI_MS_Console.RockService.getSSLBRequest();
            QRST_DI_MS_Console.RockService.getSSLBResponse retVal = ((QRST_DI_MS_Console.RockService.RockServicePortType)(this)).getSSLB(inValue);
            return retVal.@return;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        QRST_DI_MS_Console.RockService.getGPSJResponse QRST_DI_MS_Console.RockService.RockServicePortType.getGPSJ(QRST_DI_MS_Console.RockService.getGPSJRequest request) {
            return base.Channel.getGPSJ(request);
        }
        
        public string getGPSJ(string fseq) {
            QRST_DI_MS_Console.RockService.getGPSJRequest inValue = new QRST_DI_MS_Console.RockService.getGPSJRequest();
            inValue.fseq = fseq;
            QRST_DI_MS_Console.RockService.getGPSJResponse retVal = ((QRST_DI_MS_Console.RockService.RockServicePortType)(this)).getGPSJ(inValue);
            return retVal.@return;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        QRST_DI_MS_Console.RockService.getTypesResponse QRST_DI_MS_Console.RockService.RockServicePortType.getTypes(QRST_DI_MS_Console.RockService.getTypesRequest request) {
            return base.Channel.getTypes(request);
        }
        
        public string getTypes() {
            QRST_DI_MS_Console.RockService.getTypesRequest inValue = new QRST_DI_MS_Console.RockService.getTypesRequest();
            QRST_DI_MS_Console.RockService.getTypesResponse retVal = ((QRST_DI_MS_Console.RockService.RockServicePortType)(this)).getTypes(inValue);
            return retVal.@return;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        QRST_DI_MS_Console.RockService.getQueryRocksResponse QRST_DI_MS_Console.RockService.RockServicePortType.getQueryRocks(QRST_DI_MS_Console.RockService.getQueryRocksRequest request) {
            return base.Channel.getQueryRocks(request);
        }
        
        public string getQueryRocks(string dts, string dte, string maxLat, string maxLng, string minLat, string minLng, string ykmc, string yklb, string ykzl, string sslb, string pagenum, string pagesize) {
            QRST_DI_MS_Console.RockService.getQueryRocksRequest inValue = new QRST_DI_MS_Console.RockService.getQueryRocksRequest();
            inValue.dts = dts;
            inValue.dte = dte;
            inValue.maxLat = maxLat;
            inValue.maxLng = maxLng;
            inValue.minLat = minLat;
            inValue.minLng = minLng;
            inValue.ykmc = ykmc;
            inValue.yklb = yklb;
            inValue.ykzl = ykzl;
            inValue.sslb = sslb;
            inValue.pagenum = pagenum;
            inValue.pagesize = pagesize;
            QRST_DI_MS_Console.RockService.getQueryRocksResponse retVal = ((QRST_DI_MS_Console.RockService.RockServicePortType)(this)).getQueryRocks(inValue);
            return retVal.@return;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        QRST_DI_MS_Console.RockService.getSubTypesResponse QRST_DI_MS_Console.RockService.RockServicePortType.getSubTypes(QRST_DI_MS_Console.RockService.getSubTypesRequest request) {
            return base.Channel.getSubTypes(request);
        }
        
        public string getSubTypes() {
            QRST_DI_MS_Console.RockService.getSubTypesRequest inValue = new QRST_DI_MS_Console.RockService.getSubTypesRequest();
            QRST_DI_MS_Console.RockService.getSubTypesResponse retVal = ((QRST_DI_MS_Console.RockService.RockServicePortType)(this)).getSubTypes(inValue);
            return retVal.@return;
        }
    }
}