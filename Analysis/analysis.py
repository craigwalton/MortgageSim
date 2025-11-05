"""Common functions used by analysis notebooks."""

import os
from itertools import permutations
from typing import cast

import pandas as pd
import plotly.express as px
import plotly.graph_objects as go
import plotly.offline as offline
from inflection import titleize


def load_csv(name: str) -> pd.DataFrame:
    return pd.read_csv(f"../Results/{name}.csv")


def load_csv_from_vars(*args: str) -> pd.DataFrame:
    for p in list(permutations(args)):
        joined = "-".join(p)
        name = f"../Results/{joined}.csv"
        if os.path.exists(name):
            return pd.read_csv(name)
    raise FileNotFoundError(f"Could not find a CSV file for {args}")


def infer_units(var: str) -> str:
    name = var.lower()
    if "rate" in name or "increase" in name:
        return "%"
    if "duration" in name or "term" in name:
        return "years"
    return "£"


def infer_multiplier(var: str) -> int:
    return infer_multiplier_from_units(infer_units(var))


def infer_multiplier_from_units(units: str) -> int:
    return 100 if units == "%" else 1


def get_axis_title(var: str) -> str:
    return f"{titleize(var)} ({infer_units(var)})"


def add_2d_baseline(fig: go.Figure, var_1: str, var_2: str) -> None:
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


def save(fig: go.Figure, name: str) -> None:
    fig.update_layout(
        font={"size": 8},
        margin={"l": 0, "r": 0, "t": 0, "b": 0},
    )
    offline.plot(
        fig,
        filename=f"../docs/plots/{name}.html",
        auto_open=False,
        include_plotlyjs="cdn",
    )


def get_color(var: str) -> str:
    vars = [
        "initialPropertyValue",
        "propertyValueYearlyIncrease",
        "mortgageInterestRate",
        "initialMonthlyRentPrice",
        "rentPriceYearlyIncrease",
        "savingsInterestRate",
    ]
    return cast(str, px.colors.qualitative.Plotly[vars.index(var)])
