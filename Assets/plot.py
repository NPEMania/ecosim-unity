from matplotlib import pyplot as plot
from pandas import read_csv as pcsv

df = pcsv('gen_stats.csv')
print(df)