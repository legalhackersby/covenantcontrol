import React, { Component } from 'react';
import { Panel } from 'react-bootstrap';

export class Task extends Component {
    displayName = Task.name

    render() {
        return (
          <Panel>
              <Panel.Body>
                  Task description
              </Panel.Body>
          </Panel>
        );
    }
}