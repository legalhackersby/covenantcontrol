import React, { Component } from 'react';
import axios from 'axios'
import { Col, Grid, Panel, Row, Form } from 'react-bootstrap';
import './Home.css'
import { CovenantList } from './CovenantList';

import { Config } from '../Config';

export class Home extends Component {
    displayName = Home.name

    constructor(props) {
        super(props);
        this.state = {
            covenants: [{
                description: 'Срок действия договора устанавливается до 31.08.2019 года',
                date: '31.08.2019'
            }, {
                description: 'Арендатор предоставляет Арендодателю в срок до 20 (двадцатого) числа отчетного месяца копию платежного поручения о перечислении суммы арендной платы по адресу: Республика Беларусь, город Минск, ул. Радужная, 25. Копия платежного поручения должна содержать отметку обслуживающего банка о проведении платежа.',
            }]
        };
    }

    handleSelect(event) {

        let file = event.target.files[0];

        const data = new FormData()
        data.append('file', file)

        axios
            .post(Config.apiHost + "/api/upload", data, {
                onUploadProgress: ProgressEvent => {
                    console.log(ProgressEvent);
                },
            })
            .then(response => {
                console.log(response);
            })
    }

    componentWillMount() {
        axios.get(`${Config.apiHost}/api/document/1`)
             .then(res => this.setState({ ...this.state, fileContent: res.data }));
    }

     render() {
        return (
            <Grid fluid className={'content-container'}>
                <Row>
                    <Col sm={8}>
                        <Panel>
                            <Panel.Heading>
                                <Row>
                                    <Col sm={12}>
                                        <Form id="uploadForm" method="POST" action="http://localhost:56248/api/Upload">
                                            <div className="file-upload-container">
                                                <label className="file-upload btn btn-primary">
                                                    Upload <input type="file" onChange={this.handleSelect} />
                                                </label>
                                            </div>
                                        </Form>
                                    </Col>
                                </Row>
                            </Panel.Heading>
                            <Panel.Body>
                                <div dangerouslySetInnerHTML={{__html: this.state.fileContent}}></div>
                            </Panel.Body>
                    </Panel>
                </Col>
                <Col sm={4}>
                    <CovenantList covenants={this.state.covenants}/>
                </Col>
            </Row>
        </Grid>
    );
  }
}
