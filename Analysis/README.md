## Environment

The environment is managed using Conda:

    conda env create -f environment.yml
    conda activate MortgageSim


## Formatting

Python files and Jupyter notebooks are formatted using black and isort:

    black .
    isort .


## Exporting

To export the jupyter notebooks to HTML:

    conda activate MortgageSim
    jupyter nbconvert --execute --no-input --to html <name>.ipynb
