import fetch from 'isomorphic-fetch';

export const START_DATE = 'START_DATE';
export const END_DATE = 'END_DATE';

export const REFRESH = 'REFRESH';
export const SEARCH_RESULTS = 'SEARCH_RESULTS';
export const RECEIVE_PLAYER_INFO = 'RECEIVE_PLAYER_INFO';

export const DAILY_SUMMARY = 'DAILY_SUMMARY';

export const REQUEST_POSTS = 'REQUEST_POSTS';
export const RECEIVE_PAGE = 'RECEIVE_PAGE';

export const SET_LEAGUE = 'SET_LEAGUE';
export const RECEIVE_NIGHTS = 'RECEIVE_NIGHTS';
export const RECEIVE_LEAGUES= 'RECEIVE_LEAGUES';

export const ERROR = 'ERROR';

const _mamageTeamUrl = '/api/manageteam';

export function setStartDate(dateObject) {
  return { type: START_DATE, item: dateObject };
}

export function setEndDate(dateObject) {
  return { type: END_DATE, item: dateObject };
}

function searchResults(response) {
  console.log('searchResults', response);
  return {
    type: SEARCH_RESULTS,
    item: response,
    receivedAt: Date.now()
  };
}

function receivePlayerInfo(response) {
  console.log('receivePlayerInfo', response);
  return {
    type: RECEIVE_PLAYER_INFO,
    item: response,
    receivedAt: Date.now()
  };
}

function receivePage(response) {
  console.log('receivePage', response);
  return {
    type: RECEIVE_PAGE,
    item: response,
    receivedAt: Date.now()
  };
}

function setLeague(leagueName, leagueId) {
  console.log('setLeague', leagueName);
  return {
    type: SET_LEAGUE,
    item: {level: leagueName, leagueId:leagueId},
    receivedAt: Date.now()
  };
}

function receiveNights(response, sessionId, sessionName) {
  console.log('receiveNights', response);
  return {
    type: RECEIVE_NIGHTS,
    item: {nights: response.nights, sessionId:sessionId, sessionName:sessionName},
    receivedAt: Date.now()
  };
}

function receiveLeagues(response, sessionId, nightName) {
  console.log('receiveNights', response);
  return {
    type: RECEIVE_LEAGUES,
    item: {leagues: response.leagues, sessionId:sessionId, nightName:nightName},
    receivedAt: Date.now()
  };
}




function error(response) {
  console.log('receivePosts', response);
  return {
    type: ERROR,
    item: response,
    receivedAt: Date.now()
  };
}


// Meet our first thunk action creator!
// Though its insides are different, you would use it just like any other action creator:
// store.dispatch(fetchPosts('reactjs'))

export function fetchSummary(startDate, endDate) {

  // Thunk middleware knows how to handle functions.
  // It passes the dispatch method as an argument to the function,
  // thus making it able to dispatch actions itself.

  return function (dispatch) {

    // First dispatch: the app state is updated to inform
    // that the API call is starting.

    dispatch(fetchData(startDate, endDate));

    // The function called by the thunk middleware can return a value,
    // that is passed on as the return value of the dispatch method.

    // In this case, we return a promise to wait for.
    // This is not required by thunk middleware, but it is convenient for us.

    return fetch(`${_summaryUrl}/${startDate}/${endDate}`)
    .then(function(response) {
        if (response.status >= 400) {
            throw new Error("Bad response from server");
        }
        var json =  response.json();
        console.log('response', json);
        return json;
    })
    .then(item =>

      // We can dispatch many times!
      // Here, we update the app state with the results of the API call.

      dispatch(receiveSummary(item))
    )
    .catch(function(ex) {
      dispatch(error(ex));
    });

      // In a real world app, you also want to
      // catch any error in the network call.
  };
}

export function fetchLeague(leagueLevel, leagueId) {

  return function (dispatch) {

      console.log('setLeague', leagueLevel, leagueId);

      dispatch(setLeague(leagueLevel, leagueId))
  };
}


export function fetchSessionNights(sessionId, sessionName) {

  return function (dispatch) {

    //dispatch(fetchData(startDate, endDate));

    return fetch(`${_mamageTeamUrl}/session-nights/${sessionId}`)
    .then(function(response) {
        if (response.status >= 400) {
            throw new Error("Bad response from server");
        }
        var json =  response.json();
        console.log('response', json);
        return json;
    })
    .then(item =>
      dispatch(receiveNights(item, sessionId, sessionName))
    )
    .catch(function(ex) {
      dispatch(error(ex));
    });

  };
}

export function fetchNightLeagues(sessionId, nightName) {

  return function (dispatch) {

    //dispatch(fetchData(startDate, endDate));

    return fetch(`${_mamageTeamUrl}/night-leagues/${sessionId}/${nightName}`)
    .then(function(response) {
        if (response.status >= 400) {
            throw new Error("Bad response from server");
        }
        var json =  response.json();
        console.log('response', json);
        return json;
    })
    .then(item =>
      dispatch(receiveLeagues(item, sessionId, nightName))
    )
    .catch(function(ex) {
      dispatch(error(ex));
    });

  };
}

export function fetchMoveTeam(teamId,oldLeagueId,newLeagueId, sessionId) {

  return function (dispatch) {

    //dispatch(fetchData(startDate, endDate));

    return fetch(`${_mamageTeamUrl}/move-team/${teamId}/${oldLeagueId}/${newLeagueId}/${sessionId}`)
    .then(function(response) {
        if (response.status >= 400) {
            throw new Error("Bad response from server");
        }
        var json =  response.json();
        console.log('response', json);
        return json;
    })
    .then(item =>
      dispatch(receivePage(item))
    )
    .catch(function(ex) {
      dispatch(error(ex));
    });

  };
}

export function fetchCopyTeam(teamId,oldLeagueId,newLeagueId, sessionId) {

  return function (dispatch) {

    //dispatch(fetchData(startDate, endDate));

    return fetch(`${_mamageTeamUrl}/copy-team/${teamId}/${oldLeagueId}/${newLeagueId}/${sessionId}`)
    .then(function(response) {
        if (response.status >= 400) {
            throw new Error("Bad response from server");
        }
        var json =  response.json();
        console.log('response', json);
        return json;
    })
    .then(item =>
      dispatch(receivePage(item))
    )
    .catch(function(ex) {
      dispatch(error(ex));
    });

  };
}


export function fetchRemove(teamMemberId,teamId,leagueId, sessionId) {

  return function (dispatch) {

    //dispatch(fetchData(startDate, endDate));

    return fetch(`${_mamageTeamUrl}/remove/${teamMemberId}/${teamId}/${leagueId}/${sessionId}`)
    .then(function(response) {
        if (response.status >= 400) {
            throw new Error("Bad response from server");
        }
        var json =  response.json();
        console.log('response', json);
        return json;
    })
    .then(item =>
      dispatch(receivePage(item))
    )
    .catch(function(ex) {
      dispatch(error(ex));
    });

  };
}

export function fetchSetCaptain(teamMemberId,teamId,leagueId, sessionId) {

  return function (dispatch) {

    //dispatch(fetchData(startDate, endDate));

    return fetch(`${_mamageTeamUrl}/set-captain/${teamMemberId}/${teamId}/${leagueId}/${sessionId}`)
    .then(function(response) {
        if (response.status >= 400) {
            throw new Error("Bad response from server");
        }
        var json =  response.json();
        console.log('response', json);
        return json;
    })
    .then(item =>
      dispatch(receivePage(item))
    )
    .catch(function(ex) {
      dispatch(error(ex));
    });

  };
}

export function fetchPlayerInfo(playerId) {

  return function (dispatch) {

    //dispatch(fetchData(startDate, endDate));

    return fetch(`${_mamageTeamUrl}/player-info/${playerId}`)
    .then(function(response) {
        if (response.status >= 400) {
            throw new Error("Bad response from server");
        }
        var json =  response.json();
        console.log('response', json);
        return json;
    })
    .then(item =>
      dispatch(receivePlayerInfo(item))
    )
    .catch(function(ex) {
      dispatch(error(ex));
    });

  };
}

export function fetchSearchPeople(searchTerm,searchType) {

  return function (dispatch) {

    //dispatch(fetchData(startDate, endDate));
    var serchTerms = searchTerm.split(",");

    return fetch(`/api/search-people/${serchTerms[0]}/${serchTerms[1]}/${searchType}`)
    .then(function(response) {
        if (response.status >= 400) {
            throw new Error("Bad response from server");
        }
        var json =  response.json();
        console.log('response', json);
        return json;
    })
    .then(item =>
      dispatch(searchResults(item))
    )
    .catch(function(ex) {
      dispatch(error(ex));
    });

  };
}

export function fetchAddPlayer(playerId,teamId,leagueId,sessionId) {

  return function (dispatch) {

    //dispatch(fetchData(startDate, endDate));

    return fetch(`${_mamageTeamUrl}/add-player/${playerId}/${teamId}/${leagueId}/${sessionId}`)
    .then(function(response) {
        if (response.status >= 400) {
            throw new Error("Bad response from server");
        }
        var json =  response.json();
        console.log('response', json);
        return json;
    })
    .then(item =>
      dispatch(receivePage(item))
    )
    .catch(function(ex) {
      dispatch(error(ex));
    });

  };
}



export function fetchSaveTeamNumber(teamNumber,teamId, leagueId, sessionId) {

  const params = {
    headers: {
      'Accept': 'application/json',
      'Content-Type': 'application/json'
    },
    method: "POST",
    body: JSON.stringify({teamNumber: teamNumber, teamId: teamId, leagueId:leagueId , sessionId:sessionId })
  }

  return function (dispatch) {

    //dispatch(fetchData(startDate, endDate));

    return fetch(`${_mamageTeamUrl}/save-team-number`, params)
    .then(function(response) {
        if (response.status >= 400) {
            throw new Error("Bad response from server");
        }
        var json =  response.json();
        console.log('response', json);
        return json;
    })
    .then(item =>
      dispatch(receivePage(item))
    )
    .catch(function(ex) {
      dispatch(error(ex));
    });

  };
}

export function fetchUpdatePlayerInfo(selectedPlayer,teamId, leagueId, sessionId) {

  const params = {
    headers: {
      'Accept': 'application/json',
      'Content-Type': 'application/json'
    },
    method: "POST",
    body: JSON.stringify({selectedPlayer: selectedPlayer, teamId: teamId, leagueId:leagueId , sessionId:sessionId })
  }

  return function (dispatch) {

    //dispatch(fetchData(startDate, endDate));

    return fetch(`${_mamageTeamUrl}/update-player-info`, params)
    .then(function(response) {
        if (response.status >= 400) {
            throw new Error("Bad response from server");
        }
        var json =  response.json();
        console.log('response', json);
        return json;
    })
    .then(item =>
      dispatch(receivePlayerInfo(item))
    )
    .catch(function(ex) {
      dispatch(error(ex));
    });

  };
}
