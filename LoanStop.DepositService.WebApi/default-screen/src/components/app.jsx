import React from 'react';
import { connect, Provider } from 'react-redux';

import AppDefaultScreen from './containers/app-default-screen';

const Root = React.createClass({

	render() {

	    return (
			<Provider store={this.props.store}>
				<AppDefaultScreen />
			</Provider>
		);
	}
});

const App = connect()(Root);

export default App;
