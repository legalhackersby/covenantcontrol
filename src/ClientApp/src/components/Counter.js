import React, { Component } from 'react';
import { connect } from 'react-redux';
import { increment } from '../state/actions';
import { withRouter } from 'react-router-dom';

class Counter extends Component {
  displayName = Counter.name;

  render() {
    return (
      <div>
        <h1>Counter</h1>

        <p>This is a simple example of a React component.</p>

        <p>Current count: <strong>{this.props.counter.counter}</strong></p>

        <button onClick={this.props.increment}>Increment</button>
      </div>
    );
  }
}

export default withRouter(connect(
    store => ({ counter: store.counter }),
    { increment }
)(Counter));

