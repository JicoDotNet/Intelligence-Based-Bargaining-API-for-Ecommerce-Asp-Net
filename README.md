
# Intelligence Based Bargaining Engine for Ecommerce

## About

Bargaining plays a very important role in the mind of the customers to get them higher quality products, better customer services at lower prices. It is an invariable mindset and nature amongst Indian masses which rather plays an important role in buyer seller relationship. Better bargaining leads the customer to visit the store over and over again; in this case the customer will use the app invariable times.
We are providing a platform where any Ecommerce app can consume this service by **REST API** and will get AI based real time Bargaining features. 


## Get Started/Purpose

Purpose of this server to serve any ecommerce the Bargain taste. 

Bargaining help buyers and sellers negotiate the specifics of the deal and eventually come to an agreement. This agreement is highly necessary to build trust and increase sales at the same time. Unfortunately, online Bargaining facilities while purchasing any materials is unheard of. Moreover, a customer can find various pricing in various website/app but not be comfortable to use all of them neither its feasible to compare by going through all this API Service.

## API Reference

#### Get next negotiate value with massage

```http
  POST: /api/Negotiator/Negotiate
```
#### Request - Body (Param)

| Parameter | Type     |  Required  |Description |
| :-------- | :------- | :---------- |:--- |
| `TokenKey` | `string` | **✓**|Your API Auth key |
| `Tenant` | `string` |**✓** |Ecommerce Tenant Name |
| `CustomerId` | `number` |**✓** |Unique Customer Id |
| `ProductId` | `number` |**✓** |Unique Product Id |
| `ProposedCost` | `number` |**✓** |Customer Proposed Price |
| `ThresholdPrice` | `number` |**✓** |Lowest value of Product |
| `OfferPrice` | `number` |**✓** |Display Price od Product |


#### Request - JSON
```json
{
  "tokenKey": "Your_Default_Token_goes_here",
  "tenant": "string",
  "customerId": 1,
  "productId": 1,
  "proposedCost": 50,
  "thresholdPrice": 80,
  "offerPrice": 100
}
```

#### Response 

```json
{
  "IsSuccess": true,
  "StatusCode": 200,
  "Message": "Success",
  "_response": {
    "Message": "Negotiated Massage Comes Here",
    "NegotiatedCost": {
      "CustomerId": 1,
      "ProductId": 1,
      "NegotiateTime": "2022-12-20T20:58:08.9645334Z",
      "NegotiateTimeStamp": 1671569888714,
      "ProposedPrice": 50,
      "OfferedPrice": 94
    }
  }
}
```

## Tech Stack

- Asp.Net
- [Azure Table Storage](https://learn.microsoft.com/en-us/azure/storage/tables/table-storage-overview)
- C#

## Prerequisites
- Windows 10 0r 11 with [Visual Studio 2022](https://visualstudio.microsoft.com/vs/)
- [.NET 8.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)

After installing prerequisites, restart Visual Studio

## Usage/Examples
```csharp
//-- Change the Connection String of Azure Table Storage from "GenericLogic" class
public static string AzureStorageConnectionString { get { return "Your_Azure_Storage_Connection_String"; } }
public static string DefaultToken { get { return "Your_Default_Token_goes_here"; } }
```

## License

[GPL-3.0](https://choosealicense.com/licenses/gpl-3.0/)