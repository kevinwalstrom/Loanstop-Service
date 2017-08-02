import React, { PropTypes } from 'react';
import { render } from 'react-dom';
import { connect } from 'react-redux';

import { fetchSessionNights, fetchNightLeagues, fetchLeague, fetchCopyTeam, fetchMoveTeam, fetchRemove, fetchSetCaptain, fetchPlayerInfo, fetchSearchPeople, fetchAddPlayer, fetchSaveTeamNumber, fetchUpdatePlayerInfo} from '../actions/header.es';

import DefaultScreen from '../components/default-screen';

const mapStateToProps = (state) => {

  console.log('default screen : mapStateToProps', state );

  return {
      payments : state.payments,
      transaction : state.transaction,
      orignationFee : state.orignationFee,
      interest : state.interest,
      serviceFee : state.serviceFee,
      nSFFee : state.nSFFee,
      appliedDate : state.transaction.status === "Closed" ? state.transaction.DateCleared : '',
      appliedLoanAmount : state.transaction.status === "Closed" ? state.transaction.AmountDispursed : '',
      appliedOrignationFee : state.transaction.status === "Closed" ? state.orignationEarned : '',
      appliedInterest : state.transaction.status === "Closed" ? state.appliedInterest : '',
      appliedServiceFees : state.transaction.status === "Closed" ? state.appliedServiceFees : '',
      appliedNSF : state.transaction.status === "Closed" ? state.nSFFee : '',
      sumOfPayments : state.sumOfPayments,
      finalDueDate : state.finalDueDate
  };
};


const mapDispatchToProps = (dispatch, ownProps, state) => {
  return {
    handleSessionClick: (sessionId, sessionName) => {
      console.log('handleSessionClick', sessionId);
      dispatch(fetchSessionNights(sessionId, sessionName));
    },
    handleNightClick: (sessionId, nightName) =>{
      console.log('handleNightClick', sessionId);
      dispatch(fetchNightLeagues(sessionId, nightName));
    },
    handleLeagueClick:(leagueLevel, leagueId) => {
      console.log('handleLeagueClick', leagueId);
      dispatch(fetchLeague(leagueLevel, leagueId));
    },
    handleCopyTeam:(teamId, oldLeagueId, newLeagueId, sessionId) => {
      console.log('handleCopyTeam', teamId, oldLeagueId, newLeagueId, sessionId);
      dispatch(fetchCopyTeam(teamId, oldLeagueId, newLeagueId, sessionId));
    },
    handleMoveTeam:(teamId, oldLeagueId, newLeagueId, sessionId) => {
      console.log('handleMoveTeam', teamId, oldLeagueId, newLeagueId, sessionId);
      dispatch(fetchMoveTeam(teamId, oldLeagueId, newLeagueId, sessionId));
    },
    handleRemoveClick:(teamMemberId, teamId, leagueId, sessionId) => {
      console.log('handleRemoveClick',teamMemberId, teamId, leagueId, sessionId);
      dispatch(fetchRemove(teamMemberId, teamId, leagueId, sessionId));
    },
    handleSetCaptainClick:(teamMemberId, teamId, leagueId, sessionId) => {
      console.log('handleSetCaptainClick', teamId, leagueId, sessionId);
      dispatch(fetchSetCaptain(teamMemberId, teamId, leagueId, sessionId));
    },
    handleInfoClick:(playerId) => {
      console.log('handleInfoClick', playerId);
      dispatch(fetchPlayerInfo(playerId));
    },
    handleSearch:(searchTerm,searchType) => {
      console.log('handleSearch', searchTerm,searchType);
      dispatch(fetchSearchPeople(searchTerm,searchType));
    },
    handleSelectPerson:(playerId) => {
      console.log('handleSelectPerson',playerId);
      dispatch(fetchPlayerInfo(playerId));
    },
    handleAddPlayer:(playerId, teamId, leagueId, sessionId) => {
      console.log('handleAddPlayer',playerId, teamId, leagueId, sessionId);
      dispatch(fetchAddPlayer(playerId, teamId, leagueId, sessionId));
    },
    handleSaveTeamNumber:(value, teamId, leagueId, sessionId) => {
      console.log('handleSaveTeamNumber',value, teamId, leagueId, sessionId);
      dispatch(fetchSaveTeamNumber(value, teamId, leagueId, sessionId));
    },
    handleUpdatePlayerInfo:(selectedPlayer, teamId, leagueId, sessionId) => {
      console.log('handleUpdatePlayerInfo',selectedPlayer, teamId, leagueId, sessionId);
      dispatch(fetchUpdatePlayerInfo(selectedPlayer, teamId, leagueId, sessionId));
    },
 };
};
const AppDefaultScreen = connect(
  mapStateToProps,
  mapDispatchToProps
)(DefaultScreen);

export default AppDefaultScreen;

//handleStartDate, handleEndDate, handleButtonClick