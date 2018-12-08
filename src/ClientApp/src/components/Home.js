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
                id: 1,
                description: 'Срок действия договора устанавливается до 31.08.2019 года',
                date: '31.08.2019'
            }, {
                id: 2,
                description: 'Арендатор предоставляет Арендодателю в срок до 20 (двадцатого) числа отчетного месяца копию платежного поручения о перечислении суммы арендной платы по адресу: Республика Беларусь, город Минск, ул. Радужная, 25. Копия платежного поручения должна содержать отметку обслуживающего банка о проведении платежа.',
            }]
        };
    }

    skip(id) {
        console.log(id)
    }

    add(id) {
        console.log(id)
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
                axios.get(`${Config.apiHost}/api/document/${response.data}`)
                    .then((res => this.setState({ ...this.state, fileContent: this.processDocument(res.data) })));
            })
    }

    processDocument(documentContent) {

        let newContent = documentContent.replace(/<\s*mark[^>]*>/, '<div class="panel panel-default"><div class="panel-body"><div class="row"><div class="task-description col-sm-12">Срок действия договора устанавливается до 31.08.2019 года</div></div><div class="cov-date row"><label>31.08.2019</label></div><div class="row"><div class="col-sm-6"><button type="button" class="btn-secondary btn btn-default">Пропустить</button></div><div class="col-sm-6"><button type="button" class="btn-primary btn btn-default">Добавить</button></div></div></div></div><div style="padding: 10px 10px 10px 10px;display: -ms-flexbox;display: flex;border-radius: 10px;box-shadow: 0 8px 25px rgba(0,0,0,.05);background-color: #ACDCF3;">');
        let nc1 = newContent.replace('</mark>', '</div>');
        return nc1;
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
                                                    Upload <input type="file" onChange={this.handleSelect.bind(this)} />
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
                    {/*<CovenantList covenants={this.state.covenants} skip={this.skip} add={this.add}/>*/}
                </Col>
            </Row>
        </Grid>
    );
  }
}
