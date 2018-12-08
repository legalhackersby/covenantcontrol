import React, { Component } from 'react';
import { Panel } from 'react-bootstrap';
import { Covenant } from './Covenant';

export class CovenantList extends Component {
    displayName = CovenantList.name

    render() {
        return (
            <Panel>
                <Panel.Heading>
                    Covenants
                </Panel.Heading>
                <Panel.Body>
                    <Covenant/>
                    <Covenant/>
                </Panel.Body>
            </Panel>
        )
    }
}