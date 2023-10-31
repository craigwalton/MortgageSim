import pandas as pd
import plotly.graph_objects as go
from inflection import titleize


def load_csv(name):
    return pd.read_csv(f"../Data/{name}.csv")


def load_baseline():
    return load_csv("baseline").transpose()


def add_2d_baseline(fig, var_1, var_1_multiplier, var_2, var_2_multiplier):
    df = load_csv("baseline")
    fig.add_trace(
        go.Scatter(
            x=df[var_1] * var_1_multiplier,
            y=df[var_2] * var_2_multiplier,
            mode="markers+text",
            name="Baseline",
            text=["Baseline"],
            textposition="bottom center",
            marker=dict(color="green"),
        )
    )


def infer_multipler(units):
    return 100 if units == "%" else 1


def get_axis_title(var, units):
    return f"{titleize(var)} ({units})"
