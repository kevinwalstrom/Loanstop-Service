//import ReactDOM from 'react-dom';
import 'babel-polyfill';
import React from 'react';


//import React from 'react';
import { render } from 'react-dom';
import { Provider } from 'react-redux';
import { createStore, applyMiddleware  } from 'redux';
import thunkMiddleware from 'redux-thunk';
import createLogger from 'redux-logger';

import MonthlyApp from './components/reducers/monthly-app';
import App from './components/app';
import testData from '../data/test.json';

var _mamageTeamUrl = 'http://localhost:15525';
//var _mamageTeamUrl = 'http://localhost:50974/api/manageteam';

$("#spinner").show();

const loggerMiddleware = createLogger();


const state = getUrlParameter('state');
const store = getUrlParameter('store');
const transId = getUrlParameter('transId');

getDefaultData(state, store, transId);

function getDefaultData(state, store, transId){
    
    fetch(`${_mamageTeamUrl}/api/defaultscreen/${state}/${store}/${transId}`)
    .then(function(response) {
        if (response.status >= 400) {
            throw new Error("Bad response from server");
        }
        var json =  response.json();
        console.log('response', json);
        return json;
    })
    .then(item => {

		let store = createStore(
			MonthlyApp,
			item.Item,
			applyMiddleware(
		    	thunkMiddleware, // lets us dispatch() functions
		    	loggerMiddleware // neat middleware that logs actions
		  	)
		);

		render(<App store={store} />, document.getElementById('app-container'));

		$("#spinner").hide();

	}

      // We can dispatch many times!
      // Here, we update the app state with the results of the API call.

    )
    .catch(function(ex) {
      console.log(ex);
    });

}


function getUrlParameter(name) {
    name = name.replace(/[\[]/, '\\[').replace(/[\]]/, '\\]');
    var regex = new RegExp('[\\?&]' + name + '=([^&#]*)');
    var results = regex.exec(location.search);
    return results === null ? '' : decodeURIComponent(results[1].replace(/\+/g, ' '));
};