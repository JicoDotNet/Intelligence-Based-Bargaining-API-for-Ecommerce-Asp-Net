
# Intelligence Based Bargaining Engine for Ecommerce

## About

Bargaining plays a very important role in the mind of the customers to get them higher quality products, better customer services at lower prices. It is an invariable mindset and nature amongst Indian masses which rather plays an important role in buyer seller relationship. Better bargaining leads the customer to visit the store over and over again ; in this case the customer will use the app invariable times.

We are providing a platform where any Ecommerce app can consume this service by **REST API** and will get AI based real time Bargaining features. 


## Get Started/Purpose

Purpose of this server to serve any ecommerce the Bargain taste. 

Bargaining help buyers and sellers negotiate the specifics of the deal and eventually come to an agreement. This agreement is highly necessary to build trust and increase sales at the same time. Unfortunately, online Bargaining facilities while purchasing any materials is unheard of. Moreover, a customer can find various pricing in various website/app but not be comfortable to use all of them neither its feasible to compare by going through all this API Service.

## API Reference

#### Get next negotiate value with massage

```http
  POST: /api/Negotiator/Negotiate
```
#### Request - JSON

| Parameter | Type     |  Required  |Description |
| :-------- | :------- | :---------- |:--- |
| `TokenKey` | `string` | **✓**|Your API Auth key |
| `Tenant` | `string` |**✓** |Ecommerce Tenant Name |
| `CustomerId` | `number` |**✓** |Unique Customer Id |
| `ProductId` | `number` |**✓** |Unique Product Id |
| `ProposedCost` | `number` |**✓** |Customer Proposed Price |
| `ThresholdPrice` | `number` |**✓** |Lowest value of Product |
| `OfferPrice` | `number` |**✓** |Display Price od Product |

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
      "ProposedPrice": 12,
      "OfferedPrice": 942
    }
  }
}
```

## Tech Stack

Asp.Net, AzureTableStorage, C#

## Usage/Examples

```asp.net

Change the Connection String of Azure Table Storage from "GenericLogic" class
```



## License

[MIT](https://choosealicense.com/licenses/mit/)