import React, { Component } from 'react';
import axios from 'axios'
import { Col, Grid, Panel, Row, Form } from 'react-bootstrap';
import './Home.css'
import $ from 'jquery';
import _ from 'lodash';

import { Config } from '../Config';

const covenantTemplate = _.template(`
                          <div uid="<%=id%>" class="covenant panel panel-default">
                                 <div class="panel-heading cov-head">
                                    <div class="row">
                                     <div class="col-sm-8">
                                        <span><%=type%></span>
                                     </div>
                                     <div class="col-sm-1">
                                        <button uid="<%=id%>" type="button" class="btn btn-info btn-circle btn-ok head-remove-button-<%=id%>">
                                            <i class="glyphicon glyphicon-ok"></i>
                                        </button>
                                     </div>
                                     <div class="col-sm-1">
                                        <button uid="<%=id%>" type="button" class="btn btn-info btn-circle btn-remove">
                                            <i class="glyphicon glyphicon-remove"></i>
                                        </button>
                                     </div>                                     
                                 </div>
                                 </div>
                                 <div class="panel-collapse collapse">
                                     <div class="panel-body">
                                         <div class="row">
                                            <div class="task-description col-sm-12"><%=description%></div>
                                         </div>
                                     <div class="row action-buttons-<%=id%>">
                                         <div class="col-sm-6">
                                            <button uid="<%=id%>" type="button" class="skip btn-secondary btn btn-default">Пропустить</button>
                                         </div>
                                         <div class="col-sm-6">
                                            <button uid="<%=id%>" type="button" class="add btn-primary btn btn-default">Добавить</button>
                                         </div>
                                     </div>
                                  </div>
                                </div>                                 
                          </div>`);

export class Home extends Component {
    displayName = Home.name

    constructor(props) {
        super(props);
        this.state = {
            covenants: [{
                id: 1,
                description: 'Срок действия договора устанавливается до 31.08.2019 года',
                date: '31.08.2019',
                type: 'Сроки'
            }, {
                id: 2,
                description: 'Арендатор предоставляет Арендодателю в срок до 20 (двадцатого) числа отчетного месяца копию платежного поручения о перечислении суммы арендной платы по адресу: Республика Беларусь, город Минск, ул. Радужная, 25. Копия платежного поручения должна содержать отметку обслуживающего банка о проведении платежа.',
                date: '',
                type: 'Общий'
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
                axios.get(`${Config.apiHost}/api/document/${response.data}`)
                    .then((res => this.setState({ ...this.state, fileContent: res.data }, this.updateDocument)));
            })
    }

    updateDocument() {
        let covenants = this.state.covenants;
        for(let  i in covenants) {
            let cov = covenants[i];
            let htmlFragment = covenantTemplate(cov);
            let elem = $(`[id='${cov.id}']`);
            elem.addClass('highlight');
            let panel = $(htmlFragment).insertBefore(elem);
            panel.on('click', (event) => {

                if ($(event.target).hasClass('btn-ok')) {
                    this.add(event);
                    return
                }

                if ($(event.target).hasClass('btn-remove')) {
                    this.skip(event);
                    return
                }

                let collapsePanel = panel.children('.panel-collapse');
                if(!collapsePanel.hasClass('in')) {
                    collapsePanel.addClass('in');
                } else {
                    collapsePanel.removeClass('in');
                }

            });
        }

        $('.add').on('click', but => {
            this.add(but)
        });

        $('.skip').on('click', but => {
            this.skip(but);
        });
    }

    add(but) {
        let id = but.target.attributes['uid'].value;
        $(`.action-buttons-${id}`).replaceWith(`<div class="row"></div>`);
        $(`.head-remove-button-${id}`).remove();
        $(`[uid='${id}']`).addClass('panel-success');
        console.log(id);
    }

    skip(but) {
        let id = but.target.attributes['uid'].value;
        let elem = $(`[id='${id}']`);
        elem.removeClass('highlight');
        let coven = $(`[uid='${id}']`);
        coven.remove();
        console.log(id);
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
                                                    Загрузить <input type="file" onChange={this.handleSelect.bind(this)} />
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
