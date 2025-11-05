# Analysis

The `Analysis` directory contains Jupyter notebooks which are used to generate the Plotly plots on the web page.

## Environment

The environment is managed using [uv](https://docs.astral.sh/uv/).

To create the virtual environment:

    uv sync

Prefix commands with `uv run` to use the virtual environment e.g.:

    uv run jupyter notebook

## Linting & formatting

Python modules and Jupyter notebooks are linted and formatted with [ruff](https://docs.astral.sh/ruff/).

    uv run ruff check .
    uv run ruff format .

## Type checking

Python modules and Jupyter notebooks are type checked with Mypy. `nbqa` is used to run Mypy on `.ipynb` files.

    uv run mypy .
    uv run nbqa mypy .

## Clear output

Before committing, clear the output of the notebooks:

    uv run jupyter nbconvert --clear-output --inplace *.ipynb
