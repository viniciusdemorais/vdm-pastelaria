Funcionalidade: Listar Sabores dos Pasteis

Cenário: Quando existem sabores criados
	Dado solicitacao para listar sobres
	E existem sabores criados
	Quando listar sabores dos pasteis
	Então deverá retornar listagem de pasteis
	E listagem de sabores deve ter sido chamada
	E deve ter sido logado os pasteis encontrados

Cenário: Quando não exitem sabores criados
	Dado solicitacao para listar sobres
	E não existem sabores criados
	Quando listar sabores dos pasteis
	Então deverá retornar DadosNaoEncontradosException com mensagem 'Não existem Pasteis'
	E listagem de sabores deve ter sido chamada
	E deve ter sido logado os pasteis encontrados