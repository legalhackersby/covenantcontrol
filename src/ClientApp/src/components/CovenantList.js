import React, { Component } from 'react';
import { Panel } from 'react-bootstrap';
import { Covenant } from './Covenant';
import axios from 'axios'
import { Config } from '../Config';

export class CovenantList extends Component {
    displayName = CovenantList.name

    constructor(props) {
        super(props);
        this.state = {
            covenants: []
        };
    }

    componentWillMount() {
        axios.get(`${Config.apiHost}/api/document/${this.props.id}/covenants`)
            .then((response => {
                console.log(this.state);
                this.setState({...this.state, covenants: response.data});
            }))
    }

    render() {
        return (
            <Panel>
                <Panel.Heading>
                    Covenants
                </Panel.Heading>
                <Panel.Body>
                    {
                        this.state
                            .covenants
                            .map(x => <Covenant description={x.description} key={x.id}></Covenant>)
                    }
                </Panel.Body>
            </Panel>
        )
    }
}