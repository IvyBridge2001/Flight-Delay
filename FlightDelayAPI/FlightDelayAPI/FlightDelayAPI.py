from flask import Flask, request, jsonify
from flask_restful import Resource, Api
from sqlalchemy import create_engine
from json import dumps

#db_connect = create_engine('sqlite:///exemplo.db')
app = Flask(__name__)
api = Api(app)


class Test(Resource):
    def get(self):       
        result = 'API online'
        return jsonify(result)    

class Probability(Resource):
    def get(self, date, time, dest, orig):
        import pandas as pd
        from datetime import datetime
        a,b,c,d,e,f,g,h = date
        date1 = a + b + "/" + c + d + "/" + e + f + g + h + " " + time + ":00:00"
        date2 = datetime.strptime(date1, '%d/%m/%Y %H:%M:%S')
        path = r"C:\Users\danie\Desktop\gru_data\2019.csv"
        df = pd.read_csv(path,sep=',')
        df['Delayed'] = df['Delayed'].astype(float)
        print(df.dtypes)
        from sklearn.model_selection import train_test_split
        train_x, test_x, train_y, test_y = train_test_split(df.drop('Delayed', axis=1),df['Delayed'], test_size=0.2, random_state=42)
        
        from sklearn.ensemble import RandomForestClassifier
        
        model = RandomForestClassifier(random_state=13)
        model.fit(train_x,train_y)
        import numpy as np
        def predict_delay(departure_date_time, origin, destination):
            from datetime import datetime
        
            try:
                departure_date_time_parsed = datetime.strptime(departure_date_time, '%d/%m/%Y %H:%M:%S')
            except ValueError as e:
                return 'Error parsing date/time - {}'.format(e)
        
            month = departure_date_time_parsed.month
            day = departure_date_time_parsed.day
            day_of_week = departure_date_time_parsed.isoweekday()
            hour = departure_date_time_parsed.hour
        
            origin = origin.upper()
            destination = destination.upper()
        
            input = [{'Month': month,
                      'Day': day,
                      #'WeekDay': day_of_week,
                      'Hour': hour,
                      'Origin_EGLL': 1 if origin == 'EGLL' else 0,
                      'Origin_SBGR': 1 if origin == 'SBGR' else 0,
                      'Origin_KJFK': 1 if origin == 'KJFK' else 0,
                      'Origin_SBGL': 1 if origin == 'SBGL' else 0,
                      'Dest_EGLL': 1 if destination == 'EGLL' else 0,
                      'Dest_SBGR': 1 if destination == 'SBGR' else 0,
                      'Dest_KJFK': 1 if destination == 'KJFK' else 0,
                      'Dest_SBGL': 1 if destination == 'SBGL' else 0 }]
            print(input)
            return model.predict_proba(pd.DataFrame(input))[0][0]
        
        def main1(date2,time,orig,dest):
        	try:
        		import datetime
        		date4 = date2
        		date1 = date4 - datetime.timedelta(days=3)
        		date2 = date4 - datetime.timedelta(days=2)
        		date3 = date4 - datetime.timedelta(days=1)
        		date5 = date4 + datetime.timedelta(days=1)
        		date6 = date4 + datetime.timedelta(days=2)
        		date7 = date4 + datetime.timedelta(days=3)
        		labels = (datetime.datetime.strftime(date1,'%m/%d'), datetime.datetime.strftime(date2,'%m/%d'), datetime.datetime.strftime(date3,'%m/%d'), datetime.datetime.strftime(date4,'%m/%d'), datetime.datetime.strftime(date5,'%m/%d'), datetime.datetime.strftime(date6,'%m/%d'), datetime.datetime.strftime(date7,'%m/%d'))
        		values = (predict_delay(datetime.datetime.strftime(date1,'%d/%m/%Y %H:%M:%S'), orig, dest),
        				predict_delay(datetime.datetime.strftime(date2,'%d/%m/%Y %H:%M:%S'), orig, dest),
        				predict_delay(datetime.datetime.strftime(date3,'%d/%m/%Y %H:%M:%S'), orig, dest),
        				predict_delay(datetime.datetime.strftime(date4,'%d/%m/%Y %H:%M:%S'), orig, dest),
        				predict_delay(datetime.datetime.strftime(date5,'%d/%m/%Y %H:%M:%S'), orig, dest),
        				predict_delay(datetime.datetime.strftime(date6,'%d/%m/%Y %H:%M:%S'), orig, dest),
        				predict_delay(datetime.datetime.strftime(date7,'%d/%m/%Y %H:%M:%S'), orig, dest))
        		#alabels = np.arange(len(labels))
        		
        		# plt.bar(alabels, values, align='center', alpha=0.5)
        		# plt.xticks(alabels, labels)
        		# plt.ylabel('Probability of On-Time Arrival')
        		# plt.ylim((0.0, 1.0))
        		#for dat in range(len(labels)):
        		#  labelstring += dat
                
        		graph = {"Labels": labels, "Probabilities": values}

        		return graph
        	except Exception as e:
        		print(e)
        result = main1(date2,time,orig,dest)
        #import webbrowser
        #webbrowser.open('https://localhost:44334/')
        return jsonify(result)

api.add_resource(Test, '/test') 
api.add_resource(Probability, '/probability/<date>/<time>/<dest>/<orig>') 

if __name__ == '__main__':
    app.run()
