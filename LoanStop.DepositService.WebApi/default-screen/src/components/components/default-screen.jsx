import React from 'react';
import 'react-date-picker/index.css';
import moment from 'moment';

var DatePicker = require('react-datepicker');

export default React.createClass({

	getInitialState: function() {
	    
		let totalContracted = '';
		let appliedTotalContracted = '';

	    if (this.props.transaction !== 'Closed'){
			totalContracted = this.props.transaction.AmountDispursed + this.props.orignationFee + this.props.interest + (this.props.serviceFee * 5) + this.props.nSFFee;
	    	appliedTotalContracted = '';
	    }
	    else {
			totalContracted = '';
	    	appliedTotalContracted =  this.props.appliedTotalContracted;
	    }

	    return {
	    	totalContracted: totalContracted,
	    	appliedTotalContracted : appliedTotalContracted,
	    	owedAtFinalDueDate : totalContracted - this.props.sumOfPayments
	    };
	},


	handleBackButton(e){
		e.preventDefault();
	},


	displayNightList(nights){

		var _self = this;

		return (
	    	<div className="dropdown">
	            <button className="btn btn-default dropdown-toggle" type="button" id="dropdownMenu2" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
		          {`${this.props.night}` }
		          <span className="caret"></span>
		        </button>
		         <ul className="dropdown-menu" aria-labelledby="dropdownMenu2">
		          	{nights.map(function(item, index){
		          		return (<li 
		          					className="manage-team-list-item" 
		          					key={index} 
		          					onClick={() =>{_self.props.handleNightClick(_self.props.sessionId, item.nightName)}}>{item.nightName}</li>)
		          	})}
		        </ul>
			</div>
		)

	},


	handleAddressChange(e){
		var newState = {
			...this.state.selectedPlayer,
			address: e.target.value
		}
		this.setState({selectedPlayer:newState})
	},

	render() { 

	    return (
			<div className="default-screen-container container">
				<div className="default-screen-header row justify-content-center">
					<div className="col text-center bottom-align-text">
					</div>
					<div className="col text-center bottom-align-text">
					</div>
					<div className="col text-center bottom-align-text">
						<div style={{display:"inline-block"}}>Payoff Amount :</div><div style={{display:"inline-block"}}>0.00</div>
						<div>
						</div>				
					</div>
				</div>
				<div className="default-screen-header row justify-content-center">
					<div className="col text-center">
						<div className="d-inline text-weight-bold">Client</div>
						<div className="d-inline" style={{width:"20px"}}>  :  </div>
						<div className="d-inline">{this.props.transaction.Name}</div>
					</div>
					<div className="col text-center">
						<div className="d-inline text-weight-bold">Status</div>
						<div className="d-inline" style={{width:"20px"}}>  :  </div>
						<div style={{display:"inline-block"}}>
							<div className="dropdown">
							  <button className="btn btn-secondary dropdown-toggle default-screen-button nomargin" type="button" id="dropdownMenuButton" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false" >
							    Default
							  </button>
							  <div className="dropdown-menu" aria-labelledby="dropdownMenuButton">
							    <a className="dropdown-item" href="#">Action</a>
							    <a className="dropdown-item" href="#">Another action</a>
							    <a className="dropdown-item" href="#">Something else here</a>
							  </div>
							</div>					
						</div>
					</div>
					<div className="col text-center">
						<div></div><div></div>
						<div>
			                <DatePicker className="default-screen-button" ref="startDate" dateFormat="YYYY/MM/DD" selected={this.props.startDate} onChange={this.props.handleStartDate} />
						</div>				
					</div>
				</div>
				<div className="default-screen-table-div">
					<table>
					  <thead>
					    <tr>
					      <th></th>
					      <th></th>
					      <th></th>
					      <th></th>
					    </tr>
					  </thead>
					  <tbody>
					    <tr className="default-screen-tr" style={{lineHeight:"2.0"}}>
					      <th scope="row" className="default-screen-td">Loan Date</th>
					      <td className="default-screen-td"></td>
					      <td className="default-screen-col3">{ moment(this.props.transaction.TransDate).format('MM/DD/YYYY') }</td>
					      <td className="default-screen-col4"></td>
					      <td className="default-screen-col5"></td>
					      <td className="default-screen-col6">Date Defaulted</td>
					      <td className="default-screen-col7">{moment(this.props.transaction.DateReturned).format('MM/DD/YYYY') }</td>
					      <td className="default-screen-col8"></td>
					    </tr>
					    <tr className="default-screen-tr">
					      <th scope="row" className="default-screen-td">Loan Amount</th>
					      <td className="default-screen-td">$</td>
					      <td className="default-screen-col3">{this.props.transaction.AmountDispursed.toFixed(2)}</td>
					      <td className="default-screen-col4">{this.props.transaction.appliedLoanAmount}</td>
					      <td className="default-screen-col5"></td>
					      <td className="default-screen-col6">Final Due Date</td>
					      <td className="default-screen-col7">{moment(this.props.finalDueDate).format('MM/DD/YYYY') }</td>
					      <td className="default-screen-col8"></td>
					    </tr>
					    <tr  className="default-screen-tr">
					      <th scope="row" className="default-screen-td">Origination Fee</th>
					      <td className="default-screen-td">$</td>
					      <td className="default-screen-col3">{this.props.orignationFee.toFixed(2)}</td>
					      <td className="default-screen-col4">{this.props.appliedOrignationFee}</td>
					      <td className="default-screen-col5"></td>
					      <td className="default-screen-col6"></td>
					      <td className="default-screen-col7"></td>
					      <td className="default-screen-col8"></td>
					    </tr>
					    <tr  className="default-screen-tr">
					      <th scope="row" className="default-screen-td">Interest</th>
					      <td className="default-screen-td">$</td>
					      <td className="default-screen-col3">{this.props.interest.toFixed(2)}</td>
					      <td className="default-screen-col4">{this.props.appliedInterest}</td>
					      <td className="default-screen-col5"></td>
					      <td className="default-screen-col6"></td>
					      <td className="default-screen-col7"></td>
					      <td className="default-screen-col8"></td>
					    </tr>
					    <tr  className="default-screen-tr">
					      <th scope="row" className="default-screen-td">Service Fees</th>
					      <td className="default-screen-td">$</td>
					      <td className="default-screen-col3">{this.props.serviceFee.toFixed(2) * 5}</td>
					      <td className="default-screen-col4">{this.props.appliedServiceFees}</td>
					      <td className="default-screen-col5"></td>
					      <td className="default-screen-col6"></td>
					      <td className="default-screen-col7"></td>
					      <td className="default-screen-col8"></td>
					    </tr>
					    <tr  className="default-screen-tr">
					      <th scope="row" className="default-screen-td">NSF</th>
					      <td className="default-screen-td">$</td>
					      <td className="default-screen-col3">{this.props.nSFFee.toFixed(2)}</td>
					      <td className="default-screen-col4">{this.props.appliedNSF}</td>
					      <td className="default-screen-col5"></td>
					      <td className="default-screen-col6">Pd Before Default</td>
					      <td className="default-screen-col7">$ {this.props.sumOfPayments}</td>
					      <td className="default-screen-col8"></td>
					    </tr>
					    <tr  className="default-screen-tr">
					      <th scope="row" className="default-screen-td">Total Contracted</th>
					      <td className="default-screen-td">$</td>
					      <td className="default-screen-col3">{this.state.totalContracted.toFixed(2)}</td>
					      <td className="default-screen-col4">{this.state.appliedTotalContracted}</td>
					      <td className="default-screen-col5"></td>
					      <td className="default-screen-col6">Owed At Final Due Date</td>
					      <td className="default-screen-col7">$ {this.state.owedAtFinalDueDate}</td>
					      <td className="default-screen-col8"></td>
					    </tr>
					  </tbody>
					</table>
				</div>
				<div className="default-screen-buttons-div">
					<div><button type="button" className="btn btn-secondary default-screen-button">Print Suit Request</button></div>				
					<div><button type="button" className="btn btn-secondary default-screen-button">Original Payment Schedule</button></div>				
					<div><button type="button" className="btn btn-secondary default-screen-button">Collections</button></div>				
				</div>
				<div className="default-screen-payment-grid">
					<table className="table">
					  <thead className="thead-default">
					    <tr>
					      <th>Payment Type</th>
					      <th>Description</th>
					      <th>Date Paid</th>
					      <th>Fee</th>
					      <th>$ Paid</th>
					      <th>Balance</th>
					      <th>-</th>
					      <th>-</th>
					    </tr>
					  </thead>
					  <tbody>
					    <tr className="" >
					      <th scope="row"><input type="text" className="form-control" id="exampleInputEmail1" aria-describedby="emailHelp" /></th>
					      <td><input type="text" className="form-control" id="exampleInputEmail1" aria-describedby="emailHelp" /></td>
					      <td><input type="text" className="form-control" id="exampleInputEmail1" aria-describedby="emailHelp" /></td>
					      <td><input type="text" className="form-control" id="exampleInputEmail1" aria-describedby="emailHelp" /></td>
					      <td><input type="text" className="form-control" id="exampleInputEmail1" aria-describedby="emailHelp" /></td>
					      <td></td>
					      <td><button className="btn btn-secondary">Pay Cash</button></td>
					      <td><button className="btn btn-secondary">Apply To Balance</button></td>
					    </tr>
					   </tbody>
					</table> 			
				</div>

			</div>
		)
	}

})

