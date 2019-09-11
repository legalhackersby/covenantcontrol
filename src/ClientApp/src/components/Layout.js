import React, { Component } from 'react';
import { Col, Grid, Row } from 'react-bootstrap';
import { NavMenu } from './NavMenu';

import {HubConnectionBuilder, LogLevel, JsonHubProtocol, HttpTransportType} from '@aspnet/signalr';

export class Layout extends Component {
  displayName = Layout.name

  componentWillMount() {

    const protocol = new JsonHubProtocol();

    const transport = HttpTransportType.WebSockets;

    const options = {
      transport,
      logMessageContent: true,
      logger: LogLevel.Trace,
      accessTokenFactory: () => this.props.accessToken,
    };

    // create the connection instance
   /* let hubConnection = new HubConnectionBuilder()
      .withUrl("http://localhost:56248/notify", options)
      .withHubProtocol(protocol)
      .build();
  
  this.setState({ hubConnection }, () => {
  this.state.hubConnection
  .start()
  .then(() => console.log('Connection started!'))
  .catch(err => console.log('Error while establishing connection :('));
  
    this.state.hubConnection.on('sendToAll', (receivedMessage) => {
      console.log(receivedMessage);
      });
    });*/
  }

  render() {
    return (
      <Grid fluid>
        <Row>
          <Col sm={3}>
            <NavMenu />
          </Col>
          <Col sm={9}>
            {this.props.children}
          </Col>
        </Row>
      </Grid>
    );
  }
}
