// using System.Reflection;
// using System.Text.Json;
// using System.Text.Json.Serialization;
//
// namespace CodeChops.DomainDrivenDesign.Contracts.Polymorphism.Converters;
//
// public class PolymorphicJsonConverterFactory : JsonConverterFactory
// {
//     private object[] JsonConverterArguments { get; }
//
//     public PolymorphicJsonConverterFactory(IEnumerable<IPolymorphicContract> contracts)
//     {
//         this.JsonConverterArguments = new object[] { contracts };
//     }
//
//     public override bool CanConvert(Type typeToConvert)
//         => typeToConvert.IsAssignableTo(typeof(IPolymorphicContract));
//
//     public override JsonConverter CreateConverter(Type type, JsonSerializerOptions options)
//     {
//         var converter = (JsonConverter)Activator.CreateInstance(
//             type: typeof(PolymorphicJsonConverter<>).MakeGenericType(type),
//             bindingAttr: BindingFlags.Instance | BindingFlags.Public,
//             binder: null,
//             args: this.JsonConverterArguments,
//             culture: null)!;
//
//         return converter;
//     }
// }