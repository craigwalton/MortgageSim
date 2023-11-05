# Mortgage Simulator

A deterministic simulator written in C# comparing two options which prospective first time buyers are faced with:
- Buying a property with a mortgage
- Renting a property and investing any remaining funds.

The results of this simulation are presented as an HTML web page in `docs/` and hosted at [craigwalton.github.io/MortgageSim](https://craigwalton.github.io/MortgageSim).

The Jupyter notebooks in `Analysis/` are used to generate the Plotly plots used on the web page.

## Dependent variable (output)

The output of the simulation is the difference in net worth between the two scenarios after a period (e.g. 5 years):

```
Δ net worth = equity from property purchase scenario - equity from property rent scenario
            = (property value - outstanding loan)    - (savings balance)
```

## Independent variables (input)

- Deposit
- Initial property value
- Property value evolution
- Mortgage interest rate
- Initial rent price
- Rent price evolution
- Savings interest rate

Note that the model treats these as independent variables, but in reality there are economic dependencies between
them.

In the rent scenario, the deposit is immediately paid into the savings account.

## Data Sources

The following sources were consulted to select appropriate baseline values for the independent variables.

[Historical House Prices: ONS](https://www.ons.gov.uk/economy/inflationandpriceindices/bulletins/housepriceindex/august2023)

[Historical Mortgage Inerest Rates: Statisca](https://www.statista.com/statistics/386301/uk-average-mortgage-interest-rates/)

[Historical Rent Prices: ONS](https://www.ons.gov.uk/economy/inflationandpriceindices/bulletins/indexofprivatehousingrentalprices/september2023)

[Historical Bank Rate: BoE](https://www.bankofengland.co.uk/boeapps/database/Bank-Rate.asp)

## Limitations and non-goals

This simulator takes no consideration of the affordability of either scenario. Either scenario may result
in negative equity.

Costs associated with property purchase (e.g. stamp duty, solicitor fees, mortgage product fees, maintenance fees or
mortgage early repayment fees) are not considered.

This simulator only considers the financial ramifications of the two options. It does not consider non-monetary aspects
such as the speed of property transactions, the flexibility to make home modifications, or other lifestyle factors.
