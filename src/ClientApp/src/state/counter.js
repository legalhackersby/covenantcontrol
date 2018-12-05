import { ActionTypes } from './action-types';

const initialState =  {
    counter: 0
};

export default (state = initialState, action) => {
    switch (action.type) {
        case ActionTypes.INCREMENT: return { ...state, counter: ++state.counter };
        default: return state;
    }
}