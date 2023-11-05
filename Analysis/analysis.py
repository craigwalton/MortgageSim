import os
from itertools import permutations

import pandas as pd
import plotly.express as px
import plotly.graph_objects as go
import plotly.offline as offline
from inflection import titleize


def load_csv(name):
    return pd.read_csv(f"../Data/{name}.csv")


def load_csv_from_vars(*args):
    for p in list(permutations(args)):
        joined = "-".join(p)
        name = f"../Data/{joined}.csv"
        if os.path.exists(name):
            return pd.read_csv(name)


def load_baseline():
    return load_csv("baseline")


def add_2d_baseline(fig, var_1, var_2):
    df = load_csv("baseline")
    fig.add_trace(
        go.Scatter(
            x=df[var_1] * infer_multiplier(var_1),
            y=df[var_2] * infer_multiplier(var_2),
            mode="markers+text",
            name="Baseline",
            text=["Baseline"],
            textposition="bottom center",
            marker=dict(color="green"),
            showlegend=False,
        )
    )


def infer_units(var):
    name = var.lower()
    if "rate" in name or "increase" in name:
        return "%"
    if "duration" in name or "term" in name:
        return "years"
    return "£"


def infer_multiplier(var):
    return infer_multiplier_from_units(infer_units(var))


def infer_multiplier_from_units(units):
    return 100 if units == "%" else 1


def get_axis_title(var):
    return f"{titleize(var)} ({infer_units(var)})"


def print_baseline():
    def format_value(value, units):
        match units:
            case "%":
                return f"{value:10} {units}"
            case "years":
                return f"{value:10.0f} {units}"
            case "£":
                return f"{value:10.0f} {units}"

    def print_row(name, value):
        units = infer_units(name)
        multiplier = infer_multiplier(units)
        print(f"{titleize(name):<30}{format_value(value*multiplier, units)}")

    for name, values in load_baseline().transpose().iterrows():
        print_row(name, values.iloc[0])


def save(fig, name):
    fig.update_layout(
        font={"size": 8},
        margin={"l": 0, "r": 0, "t": 0, "b": 0},
    )
    offline.plot(fig, filename=f"Presentation/plots/{name}.html", auto_open=False, include_plotlyjs="cdn")


def get_color(var):
    colors = px.colors.qualitative.Plotly
    l = [
        "initialPropertyValue",
        "propertyValueYearlyIncrease",
        "mortgageInterestRate",
        "initialMonthlyRentPrice",
        "rentPriceYearlyIncrease",
        "savingsInterestRate",
    ]
    return colors[l.index(var)]
