
var defaultProps  = {
      sessionName:'Spring',
      sessionLocation:'Island',
      sessionId:'sessionId',
      night:'night',
      level:'Level',
      leagueId:'leagueId',
      format:'format',
      gender:'gender',
      sessions: [
        {sessionName : 'Island Spring', sessionIs : 444},
        {sessionName : 'Island Summer', sessionIs : 445}
      ],
      nights: [
        {sessionName : 'Island Spring', sessionIs : 444},
        {sessionName : 'Island Summer', sessionIs : 445}
      ],
      leagues: [
        {sessionName : 'Island Spring', sessionIs : 444},
        {sessionName : 'Island Summer', sessionIs : 445}
      ],
      team :{
        name : 'test name',
        number : '1',
        captain : "captain name",
        players : [
          {firstName:"First 1", lastName:"last ", playerId: 1234},
          {firstName:"First 2", lastName:"last ", playerId: 1235},
          {firstName:"First 3", lastName:"last ", playerId: 1236},
          {firstName:"First 4", lastName:"last ", playerId: 1237},
        ]
      }
}


const ManageTeamApp = (state = defaultProps, action) => {
  
  switch (action.type) {
    case 'START_DATE':
      return {
          startDate: action.item,
          endDate: state.endDate,
          loading: false,
          summaryData: state.summaryData,
          storeData: state.storeData,
          detailData: state.detailData,
          error: false,
        };
    case 'SEARCH_RESULTS':
      return {
        sessionName: state.sessionName,
        sessionLocation:state.sessionLocation,
        sessionId:state.sessionId,
        night:state.night,
        level: state.level,
        leagueId:state.leagueId,
        newLeagueId:state.newLeagueId,
        format:state.format,
        gender:state.gender,
        sessions:state.sessions,
        nights:state.nights,
        leagues: state.leagues,
        team : state.team,
        selectedPlayer : state.selectedPlayer,
        searchResults : action.item
        };
    case 'RECEIVE_PLAYER_INFO':
      
      return {
        sessionName: state.sessionName,
        sessionLocation:state.sessionLocation,
        sessionId:state.sessionId,
        night:state.night,
        level: state.level,
        leagueId:state.leagueId,
        newLeagueId:state.newLeagueId,
        format:state.format,
        gender:state.gender,
        sessions:state.sessions,
        nights:state.nights,
        leagues: state.leagues,
        team : state.team,
        selectedPlayer : action.item,
        searchResults : state.searchResults
      };

    case 'RECEIVE_PAGE':
    
      return {
        sessionName: action.item.sessionName,
        sessionLocation:action.item.sessionLocation,
        sessionId:action.item.sessionId,
        night:action.item.night,
        level: action.item.level,
        leagueId: action.item.leagueId,
        newLeagueId:action.item.newLeagueId,
        format:action.item.format,
        gender:action.item.gender,
        sessions:action.item.sessions,
        nights:action.item.nights,
        leagues: action.item.leagues,
        team : action.item.team,
        selectedPlayer : state.selectedPlayer,
        searchResults : state.searchResults
      };

    case 'SET_LEAGUE':
    
      return {
        sessionName: state.sessionName,
        sessionLocation:state.sessionLocation,
        sessionId:state.sessionId,
        night:state.night,
        level: action.item.level,
        leagueId:state.leagueId,
        newLeagueId:action.item.leagueId,
        format:state.format,
        gender:state.gender,
        sessions:state.sessions,
        nights:state.nights,
        leagues: state.leagues,
        team : state.team,
        selectedPlayer : state.selectedPlayer,
        searchResults : state.searchResults
      };

    case 'RECEIVE_LEAGUES':
    
      return {
        sessionName: state.sessionName,
        sessionLocation:state.sessionLocation,
        sessionId:state.sessionId,
        night:action.item.nightName,
        level:'',
        leagueId:state.leagueId,
        newLeagueId:state.newleagueId,
        format:state.format,
        gender:state.gender,
        sessions:state.sessions,
        nights:state.nights,
        leagues: action.item.leagues,
        team : state.team,
        selectedPlayer : state.selectedPlayer,
        searchResults : state.searchResults
      };

    case 'RECEIVE_NIGHTS':
    
      return {
        sessionName: action.item.sessionName,
        sessionLocation:state.sessionLocation,
        sessionId:action.item.sessionId,
        night:'',
        level:'',
        leagueId:state.leagueId,
        newLeagueId:state.newleagueId,
        format:state.format,
        gender:state.gender,
        sessions:state.sessions,
        nights:action.item.nights,
        leagues:state.leagues,
        team : state.team,
        selectedPlayer : state.selectedPlayer,
        searchResults : state.searchResults
      };

    case 'ERROR':
    
      return {
        sessionName: state.sessionName,
        sessionLocation:state.sessionLocation,
        sessionId:state.sessionId,
        night:state.night,
        level:state.level,
        leagueId:state.leagueId,
        newLeagueId:state.newLeagueId,
        format:state.format,
        gender:state.gender,
        sessions:state.sessions,
        nights:state.nights,
        leagues:state.leagues,
        team :state.team,
        selectedPlayer : state.selectedPlayer,
        searchResults : state.searchResults
        };

    default:
      return state;
  }
};

export default ManageTeamApp;
