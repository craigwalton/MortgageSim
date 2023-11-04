import pandas as pd
import plotly.graph_objects as go
from inflection import titleize


def load_csv(name):
    return pd.read_csv(f"../Data/{name}.csv")


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
