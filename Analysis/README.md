## Environment

The environment is managed using [uv](https://docs.astral.sh/uv/).

To create the virtual environment:

    uv sync

Prefix commands with `uv run` to use the virtual environment e.g.:

    uv run jupyter notebook

## Formatting

Python files and Jupyter notebooks are linted and formatted with [ruff](https://docs.astral.sh/ruff/).

    uv run ruff check .
    uv run ruff format .


## Clear output

Before comitting, clear the output of the notebooks:

    jupyter nbconvert --clear-output --inplace Analysis/*.ipynb
