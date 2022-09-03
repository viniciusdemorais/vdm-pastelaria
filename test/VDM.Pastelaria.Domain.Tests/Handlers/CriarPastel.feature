Funcionalidade: Criar Pastel

Cenário: Quando sabor do pastel não existe
	Dado um novo sabor
	E sabor não existe
	Quando criar pastel
	Então deverá retornar sucesso
	E criação de pastel deve ter sido chamada

Cenário: Quando sabor do pastel já existe
	Dado um novo sabor
	E sabor existe
	Quando criar pastel
	Então deverá retornar AppException com mensagem 'Sabor já existe'
	E criação de pastel deve ter sido chamada
