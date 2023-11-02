# Mortgage Simulator

A deterministic simulator written in C# comparing two options which prospective first time buyers are faced with a
choice of:
- Buying a property with a mortgage
- Renting a property and investing any remaining funds.

The Jupyter notebooks in `analysis/` compare sensitivities of the inputs.

## Dependent variable (output)

The output of the simulation is the difference in net worth between the two scenarios after a period (e.g. 5 years):

```
Δ net worth = total equity from property purchase scenario - total equity from property rent scenario
            = (property value - outstanding loan)          - (savings balance)
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

In the rent scenario, the deposit is immediately deposited into the savings account.

## Data Sources

The following sources were consulted to select appropriate baseline values for the independent variables.

[Historical Rent Prices: ONS](https://www.ons.gov.uk/economy/inflationandpriceindices/bulletins/indexofprivatehousingrentalprices/september2023)

[Historical House Prices: ONS](https://www.ons.gov.uk/economy/inflationandpriceindices/bulletins/housepriceindex/august2023)

## Limitations and non-goals

This simulator takes no consideration of the affordability of either scenario. Either scenario may result
in negative equity.

Costs associated with property purchase (e.g. stamp duty, solicitor fees, mortgage product fees, maintenance) are not
considered.
