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
                    {
                        this.props
                            .covenants
                            .map(x => <Covenant description={x.description} date={x.date} key={x.id} skip={this.props.skip} add={this.props.add}></Covenant>)
                    }
                </Panel.Body>
            </Panel>
        )
    }
}