import pandas as pd
from matplotlib import pyplot as plot

def read_file():
    stats = open('gen_stats.txt', mode='r')
    gen = []
    score = []
    total = []
    angle = []
    range_ = []
    speed = []
    rspeed = []
    for line in stats:
        sp = line.split(' ')
        gen.append(int(sp[0]))
        score.append(int(sp[1]))
        total.append(int(sp[2]))
        angle.append(float(sp[3]))
        range_.append(float(sp[4]))
        speed.append(float(sp[5]))
        speed.append(float(sp[6]))
    data = {}
    data['gen'] = gen
    data['score'] = score
    data['total'] = total
    data['angle'] = angle
    data['range'] = range_
    data['speed'] = speed
    data['rspeed'] = rspeed
    return pd.Dataframe(data)


def main():
    df = read_file()
    df.to_csv('gen_stats.csv')

if __name__ == '__main__':
    main()