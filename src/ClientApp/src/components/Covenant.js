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
                      <Col sm={6}>
                          Дата окончания договора
                      </Col>
                      <Col sm={6}>
                          <Button className={'btn-primary'}>Добавить</Button>
                      </Col>
                  </Row>
                  <Row className={'cov-date'}>
                      <label>31.08.2018</label>
                  </Row>
              </Panel.Body>
          </Panel>
        );
    }
}