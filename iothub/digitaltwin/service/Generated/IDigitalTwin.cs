// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Azure.IoT.DigitalTwin.Service
{
    using Microsoft.Rest;
    using Models;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// DigitalTwin operations.
    /// </summary>
    public partial interface IDigitalTwin
    {
        /// <summary>
        /// Gets the properties of interfaces.
        /// </summary>
        /// <param name='digitalTwinId'>
        /// Digital Twin ID. Format of digitalTwinId is DeviceId[~ModuleId].
        /// ModuleId is optional.
        /// Example 1: "myDevice"
        /// Example 2: "myDevice~module1"
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        /// <exception cref="Microsoft.Rest.HttpOperationException">
        /// Thrown when the operation returned an invalid status code
        /// </exception>
        /// <exception cref="Microsoft.Rest.SerializationException">
        /// Thrown when unable to deserialize the response
        /// </exception>
        /// <exception cref="Microsoft.Rest.ValidationException">
        /// Thrown when a required parameter is null
        /// </exception>
        Task<HttpOperationResponse<DigitalTwinInterfaces,DigitalTwinGetAllInterfacesHeaders>> GetAllInterfacesWithHttpMessagesAsync(string digitalTwinId, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// Updates desired properties of multiple interfaces.
        /// Example URI: "digitalTwins/{digitalTwinId}/interfaces"
        /// </summary>
        /// <param name='digitalTwinId'>
        /// Digital Twin ID. Format of digitalTwinId is DeviceId[~ModuleId].
        /// ModuleId is optional.
        /// Example 1: "myDevice"
        /// Example 2: "myDevice~module1"
        /// </param>
        /// <param name='interfacesPatchInfo'>
        /// Multiple interfaces desired properties to update.
        /// </param>
        /// <param name='ifMatch'>
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        /// <exception cref="Microsoft.Rest.HttpOperationException">
        /// Thrown when the operation returned an invalid status code
        /// </exception>
        /// <exception cref="Microsoft.Rest.SerializationException">
        /// Thrown when unable to deserialize the response
        /// </exception>
        /// <exception cref="Microsoft.Rest.ValidationException">
        /// Thrown when a required parameter is null
        /// </exception>
        Task<HttpOperationResponse<DigitalTwinInterfaces,DigitalTwinUpdateMultipleInterfacesHeaders>> UpdateMultipleInterfacesWithHttpMessagesAsync(string digitalTwinId, DigitalTwinInterfacesPatch interfacesPatchInfo, string ifMatch = default(string), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// Gets the properties of given interface.
        /// Example URI:
        /// "digitalTwins/{digitalTwinId}/interfaces/{interfaceName}"
        /// </summary>
        /// <param name='digitalTwinId'>
        /// Digital Twin ID. Format of digitalTwinId is DeviceId[~ModuleId].
        /// ModuleId is optional.
        /// Example 1: "myDevice"
        /// Example 2: "myDevice~module1"
        /// </param>
        /// <param name='interfaceName'>
        /// Interface name, for example
        /// &lt;example&gt;myThermostat&lt;/example&gt;.
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        /// <exception cref="Microsoft.Rest.HttpOperationException">
        /// Thrown when the operation returned an invalid status code
        /// </exception>
        /// <exception cref="Microsoft.Rest.SerializationException">
        /// Thrown when unable to deserialize the response
        /// </exception>
        /// <exception cref="Microsoft.Rest.ValidationException">
        /// Thrown when a required parameter is null
        /// </exception>
        Task<HttpOperationResponse<DigitalTwinInterfaces,DigitalTwinGetSingleInterfaceHeaders>> GetSingleInterfaceWithHttpMessagesAsync(string digitalTwinId, string interfaceName, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// Invoke a digital twin interface command.
        /// </summary>
        /// <remarks>
        /// Invoke a digital twin interface command.
        /// </remarks>
        /// <param name='digitalTwinId'>
        /// Digital Twin ID. Format of digitalTwinId is DeviceId[~ModuleId].
        /// ModuleId is optional.
        /// Example 1: "myDevice"
        /// Example 2: "myDevice~module1"
        /// </param>
        /// <param name='interfaceName'>
        /// Interface name, for example
        /// &lt;example&gt;myThermostat&lt;/example&gt;.
        /// </param>
        /// <param name='commandName'>
        /// </param>
        /// <param name='payload'>
        /// </param>
        /// <param name='responseTimeoutInSeconds'>
        /// Response timeout in seconds.
        /// </param>
        /// <param name='connectTimeoutInSeconds'>
        /// Connect timeout in seconds.
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        /// <exception cref="Microsoft.Rest.HttpOperationException">
        /// Thrown when the operation returned an invalid status code
        /// </exception>
        /// <exception cref="Microsoft.Rest.SerializationException">
        /// Thrown when unable to deserialize the response
        /// </exception>
        /// <exception cref="Microsoft.Rest.ValidationException">
        /// Thrown when a required parameter is null
        /// </exception>
        Task<HttpOperationResponse<object,DigitalTwinInvokeInterfaceCommandHeaders>> InvokeInterfaceCommandWithHttpMessagesAsync(string digitalTwinId, string interfaceName, string commandName, object payload, int? responseTimeoutInSeconds = default(int?), int? connectTimeoutInSeconds = default(int?), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// Returns a DigitalTwin model definition for the given "id".
        /// If "expand" is present in the query parameters and "id" is for a
        /// device capability model then it returns
        /// the capability metamodel with expanded interface definitions.
        /// </summary>
        /// <param name='modelId'>
        /// Model id Ex:
        /// &lt;example&gt;urn:contoso:TemperatureSensor:1&lt;/example&gt;
        /// </param>
        /// <param name='expand'>
        /// Indicates whether to expand the device capability model's interface
        /// definitions inline or not.
        /// This query parameter ONLY applies to Capability model.
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        /// <exception cref="Microsoft.Rest.HttpOperationException">
        /// Thrown when the operation returned an invalid status code
        /// </exception>
        /// <exception cref="Microsoft.Rest.SerializationException">
        /// Thrown when unable to deserialize the response
        /// </exception>
        /// <exception cref="Microsoft.Rest.ValidationException">
        /// Thrown when a required parameter is null
        /// </exception>
        Task<HttpOperationResponse<object,DigitalTwinGetDigitalTwinModelHeaders>> GetDigitalTwinModelWithHttpMessagesAsync(string modelId, bool? expand = false, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));
    }
}
