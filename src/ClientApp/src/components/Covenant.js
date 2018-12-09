import React, { Component } from 'react';
import { Button, Col, Panel, Row } from 'react-bootstrap';
import './Covenant.css';

export class Covenant extends Component {
    displayName = Covenant.name

    render() {
        return (
          <Panel>
              <Panel.Heading>
                  {this.props.type}
              </Panel.Heading>
              <Panel.Body>
                  <Row>
                      <Col sm={12} className={'task-description'}>
                          {this.props.description}
                      </Col>
                  </Row>
              </Panel.Body>
          </Panel>
        );
    }
}