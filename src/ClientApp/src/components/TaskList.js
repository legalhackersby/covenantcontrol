import React, { Component } from 'react';
import { Panel } from 'react-bootstrap';
import { Task } from './Task';

export class TaskList extends Component {
    displayName = TaskList.name

    render() {
        return (
            <Panel>
                <Panel.Heading>
                    Tasks
                </Panel.Heading>
                <Panel.Body>
                    <Task/>
                    <Task/>
                </Panel.Body>
            </Panel>
        )
    }
}