public class DiscountManager {
  private readonly IAccountDiscountCalculatorFactory factory;
  private readonly ILoyaltyDiscountCalculator loyaltyDiscountCalculator;
  
  public decimal ApplyDiscount(decimal price, AccountStatus accountStatus, int timeOfHavingAccountInYears)
  {
    decimal priceAfterDiscount = 0;
    priceAfterDiscount = factory.getAccountDiscountCalculator(accountStatus).ApplyDiscount(price);
    priceAfterDiscount = loyaltyDiscountCalculator.applyDiscount(priceAfterDiscount, timeOfHavingAccountInYears);
    return priceAfterDiscount;
  }

}

public interface ILoyaltyDiscountCalculator
{
  decimal ApplyDiscount(decimal price, int timeOfHavingAccountInYears);
}
 
public class DefaultLoyaltyDiscountCalculator : ILoyaltyDiscountCalculator
{
  public decimal ApplyDiscount(decimal price, int timeOfHavingAccountInYears)
  {
    decimal discountForLoyaltyInPercentage = (timeOfHavingAccountInYears > Constants.MAXIMUM_DISCOUNT_FOR_LOYALTY) ? (decimal)Constants.MAXIMUM_DISCOUNT_FOR_LOYALTY/100 : (decimal)timeOfHavingAccountInYears/100;
    return price - (discountForLoyaltyInPercentage * price);
  }
}

public interface IAccountDiscountCalculatorFactory
{
  IAccountDiscountCalculator GetAccountDiscountCalculator(AccountStatus accountStatus);
}
 
public class DefaultAccountDiscountCalculatorFactory : IAccountDiscountCalculatorFactory
{
  public IAccountDiscountCalculator GetAccountDiscountCalculator(AccountStatus accountStatus)
  {
    IAccountDiscountCalculator calculator;
    switch (accountStatus)
    {
      case AccountStatus.NotRegistered:
        calculator = new NotRegisteredDiscountCalculator();
        break;
      case AccountStatus.SimpleCustomer:
        calculator = new SimpleCustomerDiscountCalculator();
        break;
      case AccountStatus.ValuableCustomer:
        calculator = new ValuableCustomerDiscountCalculator();
        break;
      case AccountStatus.MostValuableCustomer:
        calculator = new MostValuableCustomerDiscountCalculator();
        break;
      default:
        throw new NotImplementedException();
    }
 
    return calculator;
  }
}

public interface IAccountDiscountCalculator
{
  decimal ApplyDiscount(decimal price);
}
 
public class NotRegisteredDiscountCalculator : IAccountDiscountCalculator
{
  public decimal ApplyDiscount(decimal price)
  {
    return price;
  }
}
 
public class SimpleCustomerDiscountCalculator : IAccountDiscountCalculator
{
  public decimal ApplyDiscount(decimal price)
  {
    return price - (Constants.DISCOUNT_FOR_SIMPLE_CUSTOMERS * price);
  }
}
 
public class ValuableCustomerDiscountCalculator : IAccountDiscountCalculator
{
  public decimal ApplyDiscount(decimal price)
  {
    return price - (Constants.DISCOUNT_FOR_VALUABLE_CUSTOMERS * price);
  }
}
 
public class MostValuableCustomerDiscountCalculator : IAccountDiscountCalculator
{
  public decimal ApplyDiscount(decimal price)
  {
    return price - (Constants.DISCOUNT_FOR_MOST_VALUABLE_CUSTOMERS * price);
  }
}


public static class PriceExtensions
{
  public static decimal ApplyDiscountForAccountStatus(this decimal price, decimal discountSize)
  {
    return price - (discountSize * price);
  }
 
  public static decimal ApplyDiscountForTimeOfHavingAccount(this decimal price, int timeOfHavingAccountInYears)
  {
     decimal discountForLoyaltyInPercentage = (timeOfHavingAccountInYears > Constants.MAXIMUM_DISCOUNT_FOR_LOYALTY) ? (decimal)Constants.MAXIMUM_DISCOUNT_FOR_LOYALTY/100 : (decimal)timeOfHavingAccountInYears/100;
    return price - (discountForLoyaltyInPercentage * price);
  }
}
// Courtesy of https://www.codeproject.com/Articles/1083348/Csharp-BAD-PRACTICES-Learn-how-to-make-a-good-code
