import React, { Component } from 'react';
import { Button, Col, Panel, Row } from 'react-bootstrap';
import './Covenant.css';

export class Covenant extends Component {
    displayName = Covenant.name

    render() {
        return (
          <Panel>
              <Panel.Body>
                  <Row>
                      <Col sm={12} className={'task-description'}>
                          {this.props.description}
                      </Col>
                  </Row>
                  <Row className={'cov-date'}>
                      <label>{this.props.date}</label>
                  </Row>
                  <Row>
                      <Col sm={6}>
                          <Button className={'btn-secondary'} onClick={() => this.props.skip(this.props.key)}>Пропустить</Button>
                      </Col>
                      <Col sm={6}>
                          <Button className={'btn-primary'} onClick={this.props.add(this.props.key)}>Добавить</Button>
                      </Col>
                  </Row>
              </Panel.Body>
          </Panel>
        );
    }
}