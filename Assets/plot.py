from matplotlib import pyplot as plot
from pandas import read_csv as pcsv

def filter(group):
    return group.loc[group['score'] == group['score'].max()]

df = pcsv('gen_stats.csv')
print(df)

fdf = df.groupby('gen', as_index=False).apply(filter).reset_index(drop=True)
fig, a = plot.subplots(2, 2)

a[0][0].plot(fdf['gen'], fdf['angle'])
a[0][0].set_title('Generation (X) vs Angle (Y)')

a[0][1].plot(fdf['gen'], fdf['range'])
a[0][1].set_title('Generation (X) vs Range (Y)')

a[1][0].plot(fdf['gen'], fdf['speed'])
a[1][0].set_title('Generation (X) vs Speed (Y)')

a[1][1].plot(fdf['gen'], fdf['rspeed'])
a[1][1].set_title('Generation (X) vs Rotation Speed (Y)')

fig.tight_layout()
plot.show()