## Environment

The environment is managed using Conda:

    conda env create -f environment.yml
    conda activate MortgageSim


## Formatting

Python files and Jupyter notebooks are formatted using black and isort:

    black .
    isort .


## Clear output

Before comitting, clear the output of the notebooks:

    jupyter nbconvert --clear-output --inplace Analysis/*.ipynb
