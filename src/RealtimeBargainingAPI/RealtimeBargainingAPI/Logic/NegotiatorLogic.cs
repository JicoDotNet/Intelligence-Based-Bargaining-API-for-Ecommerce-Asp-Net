using System;
using System.Collections.Generic;
using System.Linq;
using AIBasedRealtimeBargaining.Models;
using DataAccess.AzureStorage;

namespace RealtimeBargainingAPI.Logic
{
    public sealed class NegotiatorLogic
    {
        readonly string Token = null;
		private long? CustomerId = null;
        readonly Random RandomObj;
        int PrevNegotiateCount = 0;
		RequestCommand _reqCommand = null;
        private DataAccess.AzureStorage. _tableManager;

		public NegotiatorLogic()
        {
			RandomObj = new Random();
			_tableManager = new ExecuteTableManager("NegotiatedCost", (object)GenericLogic.AzureStorageConnectionString);
        }

        public NegotiatedValue NextValue(RequestCommand command)
        {
            _reqCommand = command;
            CustomerId = command.CustomerId;

			NegotiatedCost NewNegotiatedCostObj = new NegotiatedCost();
            NegotiatedValue ReturnObj = new NegotiatedValue();

            #region NegotiatedCost Model Binding and Get/Set Product Object, ThresholdPrice etc.
            NewNegotiatedCostObj.RowKey = Token + "-" + GenericLogic.IstNow.TimeStamp();

            NewNegotiatedCostObj.ProductId = command.ProductId;
            NewNegotiatedCostObj.CustomerId = command.CustomerId;
            NewNegotiatedCostObj.Token = Token;
            NewNegotiatedCostObj.NegotiateTimeStamp = GenericLogic.IstNow.TimeStamp();
			NewNegotiatedCostObj.ProposedPrice = command.ProposedCost;
			NewNegotiatedCostObj.PartitionKey = command.Tenant;
			#endregion

			#region Get last Negotiated Cost depend on Token and/or CustomerId
			NegotiatedCost LastNegotiatedCostObj = GetLastNegotiatedCost(NewNegotiatedCostObj.ProductId);
            #endregion

            #region Negotiation

            #region if previous Negotiation Data found
            if (LastNegotiatedCostObj != null)
            {
                // Check latest count of Negotiation, here 6
                if (PrevNegotiateCount > 5)
                {
                    ReturnObj.Type = ENegotiatedMessage.MaxLimitExceeded;
                    NewNegotiatedCostObj.OfferedPrice = LastNegotiatedCostObj.OfferedPrice;
                    NewNegotiatedCostObj.EnumNegotiatedMessage = (int)ENegotiatedMessage.MaxLimitExceeded;
                    ReturnObj.NegotiatedCost = NewNegotiatedCostObj;
                    ReturnObj.Message = "You have run out of the number of possible bids you can make. Better luck next time!";
                    SetNegotiatedCost(NewNegotiatedCostObj);
                    return ReturnObj;
                }

                // Check previous proposed price is same with current or not.
                if (NewNegotiatedCostObj.ProposedPrice <= LastNegotiatedCostObj.ProposedPrice)
                {
                    ReturnObj.Type = ENegotiatedMessage.CanNotBeSame;
                    NewNegotiatedCostObj.OfferedPrice = LastNegotiatedCostObj.OfferedPrice;
                    NewNegotiatedCostObj.EnumNegotiatedMessage = (int)ENegotiatedMessage.CanNotBeSame;
                    ReturnObj.NegotiatedCost = NewNegotiatedCostObj;
                    ReturnObj.Message = "You can not propose less than or equal to last cost of Rs." + LastNegotiatedCostObj.ProposedPrice + " you proposed earlier.";
                    SetNegotiatedCost(NewNegotiatedCostObj);
                    return ReturnObj;
                }

                if (_reqCommand.ThresholdPrice < NewNegotiatedCostObj.ProposedPrice)
                {
                    ReturnObj.Type = ENegotiatedMessage.OffredPriceAccepted;
                    NewNegotiatedCostObj.OfferedPrice = NewNegotiatedCostObj.ProposedPrice;
                    NewNegotiatedCostObj.EnumNegotiatedMessage = (int)ENegotiatedMessage.OffredPriceAccepted;
                    ReturnObj.NegotiatedCost = NewNegotiatedCostObj;
                    ReturnObj.Message = "Congratulation! Your offered price is accepted.";
                    SetNegotiatedCost(NewNegotiatedCostObj);
                    return ReturnObj;
                }

                long newOfferedPrice = CalculateOfferPrice(_reqCommand.OfferPrice, 
                        _reqCommand.ThresholdPrice,
                        LastNegotiatedCostObj.OfferedPrice,
                            LastNegotiatedCostObj.ProposedPrice, NewNegotiatedCostObj.ProposedPrice);
                
                if (newOfferedPrice < NewNegotiatedCostObj.ProposedPrice)
                {
                    ReturnObj.Type = ENegotiatedMessage.OffredPriceAccepted;
                    NewNegotiatedCostObj.OfferedPrice = NewNegotiatedCostObj.ProposedPrice;
                    NewNegotiatedCostObj.EnumNegotiatedMessage = (int)ENegotiatedMessage.OffredPriceAccepted;
                    ReturnObj.NegotiatedCost = NewNegotiatedCostObj;
                    ReturnObj.Message = "Congratulation! Your offered price is accepted.";
                    SetNegotiatedCost(NewNegotiatedCostObj);
                    return ReturnObj;
                }

                if (newOfferedPrice < _reqCommand.ThresholdPrice)
                {
                    ReturnObj.Type = ENegotiatedMessage.ReachedThresholdPrice;
                    NewNegotiatedCostObj.OfferedPrice = LastNegotiatedCostObj.OfferedPrice;
                    NewNegotiatedCostObj.EnumNegotiatedMessage = (int)ENegotiatedMessage.ReachedThresholdPrice;
                    ReturnObj.NegotiatedCost = NewNegotiatedCostObj;
                    ReturnObj.Message = "Sorry! Tough luck. We are unable to give you any more discount.";
                    SetNegotiatedCost(NewNegotiatedCostObj);
                    return ReturnObj;
                }
                else
                {
                    // Check, is it last biddind scope or not. according to that it shows perfect msg.
                    if (PrevNegotiateCount == PricingMetaData.TotalBargainPossibility - 1)
                    {
                        ReturnObj.Type = ENegotiatedMessage.MaxLimitExceeded;
                        NewNegotiatedCostObj.EnumNegotiatedMessage = (int)ENegotiatedMessage.MaxLimitExceeded;
                        ReturnObj.Message = "Tough luck!";// Our last price Rs." + newOfferedPrice;
                    }
                    else
                    {
                        ReturnObj.Type = ENegotiatedMessage.NegotiationSuccess;
                        NewNegotiatedCostObj.EnumNegotiatedMessage = (int)ENegotiatedMessage.NegotiationSuccess;
                        NegotiateMessage negotiateMessage = SuccessfulNegotiateMessage();
                        ReturnObj.Message = negotiateMessage.MessageText;
                        NewNegotiatedCostObj.RowKeyNegotiatedMessage = negotiateMessage.MessageText;
                    }                    
                    NewNegotiatedCostObj.OfferedPrice = Convert.ToDouble(newOfferedPrice);
                    ReturnObj.NegotiatedCost = NewNegotiatedCostObj;
                    SetNegotiatedCost(NewNegotiatedCostObj);
                    return ReturnObj;
                }
            }
            #endregion
            #region else Previous Negotiation data not found
            else
            {
                if (_reqCommand.ThresholdPrice < NewNegotiatedCostObj.ProposedPrice)
                {
                    ReturnObj.Type = ENegotiatedMessage.OffredPriceAccepted;
                    NewNegotiatedCostObj.OfferedPrice = NewNegotiatedCostObj.ProposedPrice;
                    NewNegotiatedCostObj.EnumNegotiatedMessage = (int)ENegotiatedMessage.OffredPriceAccepted;
                    ReturnObj.NegotiatedCost = NewNegotiatedCostObj;
                    ReturnObj.Message = "Congratulation! Your offered price is accepted.";
                    SetNegotiatedCost(NewNegotiatedCostObj);
                    return ReturnObj;
                }

                long newOfferedPrice = CalculateOfferPrice(_reqCommand.OfferPrice, _reqCommand.ThresholdPrice, _reqCommand.OfferPrice);
                // if Proposed price is bigger then calculated Nageotiate cost
                if (newOfferedPrice < NewNegotiatedCostObj.ProposedPrice)
                {
                    ReturnObj.Type = ENegotiatedMessage.OffredPriceAccepted;
                    NewNegotiatedCostObj.OfferedPrice = NewNegotiatedCostObj.ProposedPrice;
                    NewNegotiatedCostObj.EnumNegotiatedMessage = (int)ENegotiatedMessage.OffredPriceAccepted;
                    ReturnObj.NegotiatedCost = NewNegotiatedCostObj;
                    ReturnObj.Message = "Congratulation! Your offered price is accepted.";
                    SetNegotiatedCost(NewNegotiatedCostObj);
                    return ReturnObj;
                }

                // Check: newOfferedPrice is smaller then ThresholdPrice or not. 
                if (newOfferedPrice < _reqCommand.ThresholdPrice)
                {
                    ReturnObj.Type = ENegotiatedMessage.ReachedThresholdPrice;
                    NewNegotiatedCostObj.OfferedPrice = _reqCommand.OfferPrice;
                    NewNegotiatedCostObj.EnumNegotiatedMessage = (int)ENegotiatedMessage.ReachedThresholdPrice;
                    ReturnObj.NegotiatedCost = NewNegotiatedCostObj;
                    SetNegotiatedCost(NewNegotiatedCostObj);
                    ReturnObj.Message = "Sorry! Tough luck! We are unable to give you any more discount.";
                    return ReturnObj;
                }
                else
                {
                    ReturnObj.Type = ENegotiatedMessage.NegotiationSuccess;
                    NewNegotiatedCostObj.OfferedPrice = Convert.ToDouble(newOfferedPrice);
                    NewNegotiatedCostObj.EnumNegotiatedMessage = (int)ENegotiatedMessage.NegotiationSuccess;
                    ReturnObj.NegotiatedCost = NewNegotiatedCostObj;
                    NegotiateMessage negotiateMessage = SuccessfulNegotiateMessage();
                    ReturnObj.Message = negotiateMessage.MessageText;
                    NewNegotiatedCostObj.RowKeyNegotiatedMessage = negotiateMessage.MessageText;
                    SetNegotiatedCost(NewNegotiatedCostObj);
                    return ReturnObj;
                }
            }
            #endregion

            #endregion
        }

        private void SetNegotiatedCost(NegotiatedCost Model)
        {
            Model.NegotiateTime = GenericLogic.IstNow;
            _tableManager.InsertEntity(Model);
        }

        private NegotiatedCost GetLastNegotiatedCost(long ProductId)
        {
            string Query = "NegotiateTimeStamp gt " + GenericLogic.IstNow.AddHours(-24).TimeStamp() + "L";
            if (CustomerId == null)
            {
                Query += @" and Token eq '" + Token + "' and ProductId eq " + ProductId;
            }
            else
            {
                Query += @" and CustomerId eq " + CustomerId.Value + " and ProductId eq " + ProductId;
            }
            List<NegotiatedCost> OldNCs = _tableManager.RetrieveEntity<NegotiatedCost>(Query);

            // if rows found
            if (OldNCs.Count > 0)
            {
                // Sorting
                OldNCs = OldNCs.OrderByDescending(a => a.NegotiateTimeStamp).ToList();

                // if last Negotiate held with in 24hrs.(86400000 TimeStamp)
                if ((GenericLogic.IstNow.TimeStamp() - OldNCs[0].NegotiateTimeStamp) < 86400000)
                {
                    PrevNegotiateCount = OldNCs.Count;
                    return OldNCs[0];
                }
            }
            return null;
        }

		private long CalculateOfferPrice(double SalePrice, double ThresholdPrice, double LastOfferPrice,
                            double? LastProposedPrice = null, double? NewProposedPrice = null)
        {
            // min = 2 & 6
            // max = 16
            double RandDiscountPercentage = 0.0; // (RandomObj.NextDouble() * (max - min) + min)
            double newOfferedPrice = SalePrice;
            if (LastProposedPrice != null && NewProposedPrice != null)
            {
                double? ProposedPriceHikePercentage = ((NewProposedPrice - LastProposedPrice) / LastProposedPrice) * 100;
                if (ProposedPriceHikePercentage <= 10)
                {
                    RandDiscountPercentage = RandomObj.NextDouble() * (6 - 2) + 2;
                }
                if (ProposedPriceHikePercentage > 10 && ProposedPriceHikePercentage <= 20)
                {
                    RandDiscountPercentage = RandomObj.NextDouble() * (8 - 6) + 6;
                }
                if (ProposedPriceHikePercentage > 20 && ProposedPriceHikePercentage <= 40)
                {
                    RandDiscountPercentage = RandomObj.NextDouble() * (10 - 8) + 8;
                }
                if (ProposedPriceHikePercentage > 40 && ProposedPriceHikePercentage <= 60)
                {
                    RandDiscountPercentage = RandomObj.NextDouble() * (12 - 10) + 10;
                }
                if (ProposedPriceHikePercentage > 60 && ProposedPriceHikePercentage <= 80)
                {
                    RandDiscountPercentage = RandomObj.NextDouble() * (14 - 12) + 12;
                }
                if (ProposedPriceHikePercentage > 80)
                {
                    RandDiscountPercentage = RandomObj.NextDouble() * (16.66 - 16) + 16;
                }
            }
            else
            {
                RandDiscountPercentage = RandomObj.NextDouble() * (16 - 6) + 6;                
            }
            if(SalePrice > ThresholdPrice && RandDiscountPercentage > 0)
            {
                newOfferedPrice = LastOfferPrice - ((SalePrice - ThresholdPrice) * RandDiscountPercentage / 100);
            }
            return Convert.ToInt64(Math.Round(newOfferedPrice));
        }

        private NegotiateMessage SuccessfulNegotiateMessage()
        {
			List<string> ListMessages = new List<string>
				{
					"Hey we are talking about Carpet of the latest design and not a bag of onions. If you have to haggle, haggle fairly!",
					"Do you know what the price of such Biscuit Jar is in the market? We are offering this at an already discounted price. How much lower do you want us to go?",
					"Hey we are talking about thi product of the latest design and not a bag of onions. If you have to haggle, haggle fairly!",
					"You will find it very expensive in the high street. Think of the convenience of buying latest products sitting at home and at such low prices! Go ahead, offer a better price",
					"Don't count your pennies and bid higher. Wanna be a good player of the game, don't you?",
					"C'mon you can do better than that. After all, it is not everyday that you get to bargain for such a nice item.",
					"That's too low as well for something as nice as that. Do you know the price of such a nice item in the market?",
					"Don't be a miser. You cannot expect to look good in such a nice and get it for nothing. I have already offered you a sizeable discount.",
					"This item can be cheap but they don't come as cheap as that! Especially not this one."
				};
			return new NegotiateMessage() { MessageText = ListMessages[RandomObj.Next(0, ListMessages.Count)], Language = "en" };
		}
    }
}
